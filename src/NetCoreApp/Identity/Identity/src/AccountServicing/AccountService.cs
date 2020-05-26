using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using ComponentDataValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Presenters;
using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Entities;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Extensions;
using Teronis.ObjectModel.Annotations;
using Teronis.Identity.AccountServicing.Datatransjects;
using System.Transactions;

namespace Teronis.Identity.AccountServicing
{
    public class AccountService
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly RoleManager<RoleEntity> roleManager;
        private readonly ILogger<AccountService>? logger;

        public AccountService(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, ILogger<AccountService>? logger = null)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<IServiceResult<RoleCreationDatatransject>> CreateRoleAsync(RoleDescriptorDatatransject roleDescriptor)
        {
            Validator.ValidateObject(roleDescriptor, new ComponentDataValidationContext(roleDescriptor), true);

            var createdRole = new Lazy<RoleCreationDatatransject>(() => new RoleCreationDatatransject() {
                Role = roleDescriptor.Role
            });

            var roleName = roleDescriptor.Role;
            RoleEntity role;

            try {
                role = await roleManager.FindByNameAsync(roleName);
            } catch (Exception error) {
                var errorMessage = $"The role '{roleName}' existence couldn't be checked";
                logger?.LogError(error, errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleCreationDatatransject>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            if (role != null) {
                return $"The role '{roleName}' has been already created"
                    .ToJsonError(AccountServiceErrorCodes.RoleAlreadyCreatedErrorCode.GetStringValue())
                    .ToServiceResultFactory<RoleCreationDatatransject>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }

            role = new RoleEntity(roleName);
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded) {
                var errorMessage = $"The role '{roleName}' could not be created";
                logger?.LogError(errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleCreationDatatransject>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            return ServiceResult<RoleCreationDatatransject>
                .SucceededWithContent(createdRole.Value)
                .WithHttpStatusCode(HttpStatusCode.OK);
        }

        public async Task<IServiceResult<UserCreationDatatransject>> CreateUserAsync(UserDescriptorDatatransject userDescriptor)
        {
            Validator.ValidateObject(userDescriptor, new ComponentDataValidationContext(userDescriptor), true);

            var lazyUserCreation = new Lazy<UserCreationDatatransject>(() => new UserCreationDatatransject() {
                UserName = userDescriptor.UserName,
                Roles = userDescriptor.Roles,
                Email = userDescriptor.Email,
                PhoneNumber = userDescriptor.PhoneNumber
            });

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var userName = userDescriptor.UserName;
            var user = await userManager.FindByNameAsync(userName);

            if (!ReferenceEquals(user, null)) {
                return $"The user '{userName}' has been already created"
                    .ToJsonError(AccountServiceErrorCodes.UserAlreadyCreatedErrorCode.GetStringValue())
                    .ToServiceResultFactory<UserCreationDatatransject>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }
            // User does not exist, we continue user creation.
            else {
                user = new UserEntity(userName) {
                    Email = userDescriptor.Email,
                    PhoneNumber = userDescriptor.PhoneNumber,
                };

                var userResult = await userManager.CreateAsync(user, userDescriptor.Password);

                if (!userResult.Succeeded) {
                    var errorMessage = $"The user '{userName}' couldn't be created";
                    logger?.LogError(errorMessage);

                    var failedCreateUserResult = errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserCreationDatatransject>()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                        .AsServiceResult();

                    foreach (var error in userResult.Errors) {
                        failedCreateUserResult.Errors!.AddError(new JsonError(error.Code, error.Description));
                    }

                    return failedCreateUserResult;
                }
            }

            ServiceResult<UserCreationDatatransject>? userCreationResult = null;
            var roleNames = userDescriptor.Roles;

            if (roleNames != null) {
                ServiceResult<UserCreationDatatransject> createRoleCouldNotBeAssignedErrorResult(string roleName)
                {
                    var errorMessage = $"The user '{userName}' couldn't be assigned to role '{roleName}'. The user creation has been aborted.";
                    logger?.LogError(errorMessage);

                    return errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserCreationDatatransject>()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                        .AsServiceResult();
                };

                foreach (var roleName in roleNames)
                    try {
                        if (string.IsNullOrWhiteSpace(roleName)) {
                            var errorMessage = $"The role '{userName}' is null or empty";
                            logger?.LogError(errorMessage);

                            userCreationResult = errorMessage
                                .ToJsonError()
                                .ToServiceResultFactory<UserCreationDatatransject>()
                                .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                                .AsServiceResult();

                            break;
                        }

                        if (user != null && !await userManager.IsInRoleAsync(user, roleName)) {
                            var result = await userManager.AddToRoleAsync(user, roleName);

                            if (!result.Succeeded) {
                                userCreationResult = createRoleCouldNotBeAssignedErrorResult(roleName);
                                break;
                            }
                        }
                    } catch {
                        userCreationResult = createRoleCouldNotBeAssignedErrorResult(roleName);
                        break;
                    }
            }

            var shouldAbortUserCreation = !(userCreationResult?.Succeeded() ?? true);

            if (shouldAbortUserCreation) {
                try {
                    // TODO: Check if it works.
                    transactionScope.Dispose();
                } catch (Exception error) {
                    var errorMessage = $"User abortion failed. The user '{userName}' couldn't be deleted from the database.";
                    logger?.LogCritical(error, errorMessage);
                }

                return ServiceResult<UserCreationDatatransject>
                    .Failed(userCreationResult!);
            } else {
                transactionScope.Complete();

                return ServiceResult<UserCreationDatatransject>
                .SucceededWithContent(lazyUserCreation.Value)
                .WithHttpStatusCode(HttpStatusCode.OK);
            }
        }
    }
}
