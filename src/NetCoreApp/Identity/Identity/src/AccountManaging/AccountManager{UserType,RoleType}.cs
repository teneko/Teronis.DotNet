using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.Identity.Entities;
using Teronis.Identity.Extensions;
using Teronis.Mvc.ServiceResulting;
using Teronis.Mvc.ServiceResulting.Generic;
using Teronis.ObjectModel.Annotations;
using ComponentDataValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Teronis.Identity.AccountManaging
{
    public class AccountManager<DbContextType, UserType, RoleType> : IAccountManager<UserType, RoleType>
        where DbContextType : DbContext
        where UserType : class, IAccountUserEntity
        where RoleType : class, IAccountRoleEntity
    {
        private readonly ErrorDetailsProvider errorDetailsProvider;
        private readonly DbContextType dbContext;
        private readonly UserManager<UserType> userManager;
        private readonly RoleManager<RoleType> roleManager;
        private readonly ILogger<AccountManager<DbContextType, UserType, RoleType>>? logger;

        public AccountManager(IOptions<AccountManagerOptions> accountManagerOptions, DbContextType dbContext, UserManager<UserType> userManager, RoleManager<RoleType> roleManager, ILogger<AccountManager<DbContextType, UserType, RoleType>>? logger = null)
        {
            errorDetailsProvider = new ErrorDetailsProvider(() => accountManagerOptions.Value.IncludeErrorDetails, logger);
            // We ensure, that this instance is not tracking user or role. But it does not
            // prevent that the user manager and the role manager are tracking them. So we
            // have to try to get first the local tracked entity before we get an untracked
            // entity. Otherwise it would cause into an exception where two instances with
            // same key are accidentally tracked.
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        private async Task<IServiceResult<RoleType>> loadRoleByNameAsync(string roleName)
        {
            try {
                var createdRoleEntity = dbContext.Set<RoleType>().Local.SingleOrDefault(x => x.RoleName == roleName) ??
                    await roleManager.FindByNameAsync(roleName);

                return ServiceResult<RoleType>
                    .Success(createdRoleEntity)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                return errorDetailsProvider.LogCriticalThenBuildAppropiateError<RoleType>(error.ToJsonErrors(), $"The role '{roleName}' could not be loaded from the database.".ToJsonErrors())
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
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

                return errorDetailsProvider.LogCriticalThenBuildAppropiateError<RoleType>(error.ToJsonErrors(), errorMessage.ToJsonErrors())
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
            }

            if (existingRoleEntity != null) {
                return $"The role '{roleName}' has been already created."
                    .ToJsonError(AccountManagerErrorCodes.RoleAlreadyCreated.GetStringValue())
                    .ToServiceResult<RoleType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest);
            }
            // TODO: Integrate IdentityResult.Errors in detailed errors
            var result = await roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded) {
                var sensitiveJsonErrors = result.ToJsonErrors();
                var insensitiveJsonErrors = $"The role '{roleName}' could not be created.".ToJsonErrors();

                return errorDetailsProvider.LogCriticalThenBuildAppropiateError<RoleType>(sensitiveJsonErrors, insensitiveJsonErrors)
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
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
                var createdUserEntity = dbContext.Set<UserType>().Local.SingleOrDefault(x => x.UserName == userName) ??
                    await userManager.FindByNameAsync(userName);

                //var createdUserEntry = dbContext.Entry(createdUserEntity);
                //createdUserEntry.State = EntityState.Detached;

                return ServiceResult<UserType>
                    .Success(createdUserEntity)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception sensitiveError) {
                var insensitiveErrorMessage = $"The user '{userName}' could not be loaded from the database.";

                return errorDetailsProvider.LogErrorThenBuildAppropiateError<UserType>(sensitiveError, insensitiveErrorMessage)
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
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
                    .ToServiceResult<UserType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest);
            }
            // User does not exist, we continue user creation.
            else {
                var insensitiveErrorMessage = $"The user '{userName}' could not be created.";

                try {
                    var userResult = await userManager.CreateAsync(userEntity, password);

                    if (!userResult.Succeeded) {
                        var resultJsonErrors = userResult.ToJsonErrors();
                        resultJsonErrors.Insert(0, insensitiveErrorMessage.ToJsonError());

                        return resultJsonErrors
                            .ToServiceResult<UserType>()
                            .WithHttpStatusCode(HttpStatusCode.BadRequest);
                    }
                } catch (Exception error) {
                    return errorDetailsProvider.LogCriticalThenBuildAppropiateError<UserType>(error, insensitiveErrorMessage)
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError);
                }
            }

            var loadUserResult = await loadUserByNameAsync(userName);

            if (!loadUserResult.Succeeded) {
                return loadUserResult;
            }

            var loadedUser = loadUserResult.Content!;
            //dbContext.Entrie(loadedUser).State = EntityState.Detached;
            IServiceResult<UserType>? userRoleAssignmentResult = null;

            if (roles != null) {
                foreach (var roleName in roles) {
                    var roleAssignmentErrorMessage = $"The user '{userName}' could not be assigned to role '{roleName}'. The user creation has been aborted.";

                    try {
                        if (string.IsNullOrWhiteSpace(roleName)) {
                            var errorMessage = $"The role '{userName}' is null or empty.";

                            userRoleAssignmentResult = errorMessage
                                .ToJsonError()
                                .ToServiceResult<UserType>()
                                .WithHttpStatusCode(HttpStatusCode.InternalServerError);

                            break;
                        }

                        if (!await userManager.IsInRoleAsync(loadedUser, roleName)) {
                            var result = await userManager.AddToRoleAsync(loadedUser, roleName);

                            if (!result.Succeeded) {
                                var resultJsonErrors = result.ToJsonErrors();
                                resultJsonErrors.Insert(0, roleAssignmentErrorMessage);

                                userRoleAssignmentResult = resultJsonErrors
                                    .ToServiceResult<UserType>()
                                    .WithHttpStatusCode(HttpStatusCode.BadRequest);

                                break;
                            }
                        }
                    } catch (Exception error) {
                        userRoleAssignmentResult = errorDetailsProvider.LogCriticalThenBuildAppropiateError<UserType>(error, roleAssignmentErrorMessage)
                            .WithHttpStatusCode(HttpStatusCode.InternalServerError);
                    }
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

                return userRoleAssignmentResult!;
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

        public async Task<IServiceResult> InvalidateRefreshTokensAsync(string userId)
        {
            try {
                var userEntity = await userManager.FindByNameAsync(userId);

                if (userEntity == null) {
                    throw new ArgumentException($"The user by id '{userId}' does not exist.");
                }

                var result = await userManager.UpdateSecurityStampAsync(userEntity);

                if (!result.Succeeded) {
                    throw result.ToAggregatedException("Invalidating the refresh tokens failed.");
                }

                return new ServiceResult(true).WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                return errorDetailsProvider.LogErrorThenBuildAppropiateError<object>(error)
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
