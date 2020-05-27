using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using ComponentDataValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Presenters;
using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Extensions;
using Teronis.ObjectModel.Annotations;
using System.Transactions;

namespace Teronis.Identity.AccountManaging
{
    public abstract class AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> : IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> 
        where UserDescriptorType : IUserDescriptor
        where UserType : class
        where RoleDescriptorType : IRoleDescriptor
        where RoleType : class
    {
        private readonly UserManager<UserType> userManager;
        private readonly RoleManager<RoleType> roleManager;
        private readonly ILogger<AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>>? logger;

        public AccountManager(UserManager<UserType> userManager, RoleManager<RoleType> roleManager, ILogger<AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>>? logger = null)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        protected abstract RoleType CreateRoleEntity(RoleDescriptorType roleDescriptor);
        protected abstract RoleCreationType CreateRoleCreationObject(RoleType roleEntity);
        protected abstract UserType CreateUserEntity(UserDescriptorType roleDescriptor);
        protected abstract UserCreationType CreateUserCreationObject(UserType userEntity);

        public async Task<IServiceResult<RoleCreationType>> CreateRoleAsync(RoleDescriptorType roleDescriptor)
        {
            Validator.ValidateObject(roleDescriptor, new ComponentDataValidationContext(roleDescriptor), true);
            var roleName = roleDescriptor.Role;
            RoleType roleEntity;

            try {
                roleEntity = await roleManager.FindByNameAsync(roleName);
            } catch (Exception error) {
                var errorMessage = $"The role '{roleName}' existence couldn't be checked";
                logger?.LogError(error, errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleCreationType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            if (roleEntity != null) {
                return $"The role '{roleName}' has been already created"
                    .ToJsonError(AccountServiceErrorCodes.RoleAlreadyCreatedErrorCode.GetStringValue())
                    .ToServiceResultFactory<RoleCreationType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }

            roleEntity = CreateRoleEntity(roleDescriptor);
            var result = await roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded) {
                var errorMessage = $"The role '{roleName}' could not be created";
                logger?.LogError(errorMessage);

                return errorMessage
                    .ToJsonError()
                    .ToServiceResultFactory<RoleCreationType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }

            var roleCration = CreateRoleCreationObject(roleEntity);

            return ServiceResult<RoleCreationType>
                .SucceededWithContent(roleCration)
                .WithHttpStatusCode(HttpStatusCode.OK);
        }

        public async Task<IServiceResult<UserCreationType>> CreateUserAsync(UserDescriptorType userDescriptor)
        {
            Validator.ValidateObject(userDescriptor, new ComponentDataValidationContext(userDescriptor), true);
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var userName = userDescriptor.UserName;
            UserType user = await userManager.FindByNameAsync(userName);

            if (!ReferenceEquals(user, null)) {
                return $"The user '{userName}' has been already created"
                    .ToJsonError(AccountServiceErrorCodes.UserAlreadyCreatedErrorCode.GetStringValue())
                    .ToServiceResultFactory<UserCreationType>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            }
            // User does not exist, we continue user creation.
            else {
                user = CreateUserEntity(userDescriptor) ?? throw new ArgumentNullException(nameof(user));

                var userResult = await userManager.CreateAsync(user, userDescriptor.Password);

                if (!userResult.Succeeded) {
                    var errorMessage = $"The user '{userName}' couldn't be created";
                    logger?.LogError(errorMessage);

                    var failedCreateUserResult = errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserCreationType>()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                        .AsServiceResult();

                    foreach (var error in userResult.Errors) {
                        failedCreateUserResult.Errors!.AddError(new JsonError(error.Code, error.Description));
                    }

                    return failedCreateUserResult;
                }
            }

            ServiceResult<UserCreationType>? userCreationResult = null;
            var roleNames = userDescriptor.Roles;

            if (roleNames != null) {
                ServiceResult<UserCreationType> createRoleCouldNotBeAssignedErrorResult(string roleName)
                {
                    var errorMessage = $"The user '{userName}' couldn't be assigned to role '{roleName}'. The user creation has been aborted.";
                    logger?.LogError(errorMessage);

                    return errorMessage
                        .ToJsonError()
                        .ToServiceResultFactory<UserCreationType>()
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
                                .ToServiceResultFactory<UserCreationType>()
                                .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                                .AsServiceResult();

                            break;
                        }

                        if (!await userManager.IsInRoleAsync(user, roleName)) {
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

                return ServiceResult<UserCreationType>
                    .Failed(userCreationResult!);
            } else {
                transactionScope.Complete();
                var userCreation = CreateUserCreationObject(user);

                return ServiceResult<UserCreationType>
                    .SucceededWithContent(userCreation)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            }
        }
    }
}
