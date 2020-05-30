using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using ComponentDataValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Presenters;
using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Extensions;
using Teronis.ObjectModel.Annotations;
using Teronis.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Teronis.Identity.AccountManaging
{
    public class AccountManager<DbContextType, UserType, RoleType> : IAccountManager<UserType, RoleType>
        where DbContextType : DbContext
        where UserType : class, IAccountUserEntity
        where RoleType : class, IAccountRoleEntity
    {
        private readonly DbContextType dbContext;
        private readonly UserManager<UserType> userManager;
        private readonly RoleManager<RoleType> roleManager;
        private readonly ILogger<AccountManager<DbContextType, UserType, RoleType>>? logger;

        public AccountManager(DbContextType dbContext, UserManager<UserType> userManager, RoleManager<RoleType> roleManager, ILogger<AccountManager<DbContextType, UserType, RoleType>>? logger = null)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        private async Task<IServiceResult<RoleType>> loadRoleByNameAsync(string roleName)
        {
            try {
                var createdRoleEntity = await roleManager.FindByNameAsync(roleName);

                return ServiceResult<RoleType>
                    .Success(createdRoleEntity)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                var errorMessage = $"The role '{roleName}' could not be loaded from the database.";
                logger?.LogCritical(error, errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }
        }

        public async Task<IServiceResult<RoleType>> CreateRoleAsync(RoleType roleEntity)
        {
            Validator.ValidateObject(roleEntity, new ComponentDataValidationContext(roleEntity), true);
            var roleName = roleEntity.RoleName;
            RoleType existingRoleEntity;

            try {
                existingRoleEntity = await roleManager.FindByNameAsync(roleName);
            } catch (Exception error) {
                var errorMessage = $"The role '{roleName}' could not be reconciled against database.";
                logger?.LogError(error, errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            if (existingRoleEntity != null) {
                return $"The role '{roleName}' has been already created."
                    .ToJsonError(AccountManagerErrorCodes.RoleAlreadyCreated.GetStringValue())
                    .ToServiceResultFactory<RoleType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }

            var result = await roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded) {
                var errorMessage = $"The role '{roleName}' could not be created";
                logger?.LogError(errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            return await loadRoleByNameAsync(roleName);
        }

        public async Task<IServiceResult<RoleType>> CreateRoleIfNotExistsAsync(RoleType roleEntity)
        {
            var result = await CreateRoleAsync(roleEntity);

            if (result.Success(AccountManagerErrorCodes.RoleAlreadyCreated.GetStringValue())) {
                return await loadRoleByNameAsync(roleEntity.RoleName);
            }

            return result;
        }

        private async Task<IServiceResult<UserType>> loadUserByNameAsync(string userName)
        {
            try {
                var createdUserEntity = await userManager.FindByNameAsync(userName);

                return ServiceResult<UserType>
                    .Success(createdUserEntity)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                var errorMessage = $"The user '{userName}' could not be loaded from the database.";
                logger?.LogCritical(error, errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<UserType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }
        }

        public async Task<IServiceResult<UserType>> CreateUserAsync(UserType userEntity, string password, string[]? roles = null)
        {
            Validator.ValidateObject(userEntity, new ComponentDataValidationContext(userEntity), true);
            using var transactionScope = await dbContext.Database.BeginTransactionAsync();
            var userName = userEntity.UserName;
            UserType existingUser = await userManager.FindByNameAsync(userName);

            if (!ReferenceEquals(existingUser, null)) {
                return $"The user '{userName}' has been already created."
                    .ToJsonError(AccountManagerErrorCodes.UserAlreadyCreated.GetStringValue())
                    .ToServiceResultFactory<UserType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }
            // User does not exist, we continue user creation.
            else {
                var userResult = await userManager.CreateAsync(userEntity, password);

                if (!userResult.Succeeded) {
                    var errorMessage = $"The user '{userName}' could not be created.";
                    logger?.LogError(errorMessage);

                    var failedCreateUserResult = errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserType>()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                        .AsServiceResult();

                    foreach (var error in userResult.Errors) {
                        failedCreateUserResult.Errors!.AddError(new JsonError(error.Code, error.Description));
                    }

                    return failedCreateUserResult;
                }
            }

            var loadUserResult = await loadUserByNameAsync(userName);

            if (!loadUserResult.Succeeded) {
                return loadUserResult;
            }

            var loadedUser = loadUserResult.Content!;
            IServiceResult<UserType>? userRoleAssignmentResult = null;

            if (roles != null) {
                ServiceResult<UserType> createRoleCouldNotBeAssignedErrorResult(string roleName)
                {
                    var errorMessage = $"The user '{userName}' could not be assigned to role '{roleName}'. The user creation has been aborted.";
                    logger?.LogError(errorMessage);

                    return errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserType>()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                        .AsServiceResult();
                };

                foreach (var roleName in roles)
                    try {
                        if (string.IsNullOrWhiteSpace(roleName)) {
                            var errorMessage = $"The role '{userName}' is null or empty.";
                            logger?.LogError(errorMessage);

                            userRoleAssignmentResult = errorMessage
                                .ToJsonError()
                                .ToServiceResultFactory<UserType>()
                                .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                                .AsServiceResult();

                            break;
                        }

                        if (!await userManager.IsInRoleAsync(loadedUser, roleName)) {
                            var result = await userManager.AddToRoleAsync(loadedUser, roleName);

                            if (!result.Succeeded) {
                                userRoleAssignmentResult = createRoleCouldNotBeAssignedErrorResult(roleName);
                                break;
                            }
                        }
                    } catch {
                        userRoleAssignmentResult = createRoleCouldNotBeAssignedErrorResult(roleName);
                        break;
                    }
            }

            var shouldAbortUserCreation = !(userRoleAssignmentResult?.Success() ?? true);

            if (shouldAbortUserCreation) {
                try {
                    // TODO: Check if it works.
                    await transactionScope.RollbackAsync();
                } catch (Exception error) {
                    var errorMessage = $"The abortion of the user creation failed. The user '{userName}' could not be deleted from the database.";
                    logger?.LogCritical(error, errorMessage);
                }

                return ServiceResult<UserType>
                    .Failure(userRoleAssignmentResult!);
            } else {
                await transactionScope.CommitAsync();

                return ServiceResult<UserType>
                    .Success(loadedUser)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            }
        }

        public async Task<IServiceResult<UserType>> CreateUserIfNotExistsAsync(UserType userEntity, string password, string[]? roles = null)
        {
            var result = await CreateUserAsync(userEntity, password, roles);

            if (result.Success(AccountManagerErrorCodes.UserAlreadyCreated.GetStringValue())) {
                return await loadUserByNameAsync(userEntity.UserName);
            }

            return result;
        }
    }
}
