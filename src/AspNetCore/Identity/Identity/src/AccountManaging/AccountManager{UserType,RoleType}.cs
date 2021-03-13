using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.AspNetCore.Identity.AccountManaging.Extensions;
using Teronis.AspNetCore.Identity.Entities;
using ComponentDataValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Teronis.AspNetCore.Identity.AccountManaging
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

        public AccountManager(
            IOptions<AccountManagerOptions> _,
            DbContextType dbContext,
            UserManager<UserType> userManager,
            RoleManager<RoleType> roleManager,
            ILogger<AccountManager<DbContextType, UserType, RoleType>>? logger = null)
        {
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

        public async Task<RoleType> GetRoleByNameAsync(string roleName)
        {
            var createdRoleEntity = dbContext.Set<RoleType>().Local.SingleOrDefault(x => x.RoleName == roleName)
                ?? await roleManager.FindByNameAsync(roleName);

            return createdRoleEntity;
        }

        public async Task<RoleType> CreateRoleAsync(RoleType roleEntity)
        {
            Validator.ValidateObject(roleEntity, new ComponentDataValidationContext(roleEntity), true);
            var roleName = roleEntity.RoleName;
            RoleType existingRoleEntity;

            try {
                existingRoleEntity = await roleManager.FindByNameAsync(roleName);
            } catch (Exception error) {
                var errorMessage = $"The role '{roleName}' could not be reconciled against database.";
                logger.LogCritical(error, errorMessage);
                throw;
            }

            if (existingRoleEntity != null) {
                var insensitiveErrorMessage = $"The role '{roleName}' has been already created.";
                throw new RoleAlreadyCreatedException(roleName, insensitiveErrorMessage);
            }
            // TODO: Integrate IdentityResult.Errors in detailed errors
            var result = await roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded) {
                var insensitiveErrorMessage = $"The role '{roleName}' could not be created.";
                var aggregatedError = result.ToKeyAggregateException(insensitiveErrorMessage);
                logger.LogCritical(aggregatedError, insensitiveErrorMessage);
                throw aggregatedError;
            }

            return await GetRoleByNameAsync(roleName);
        }

        public async Task<UserType> GetUserByNameAsync(string userName)
        {
            var createdUserEntity = dbContext.Set<UserType>().Local.SingleOrDefault(x => x.UserName == userName)
                ?? await userManager.FindByNameAsync(userName);

            return createdUserEntity;
        }

        public async Task<UserType> CreateUserAsync(UserType userEntity, string password, params string[]? roles)
        {
            Validator.ValidateObject(userEntity, new ComponentDataValidationContext(userEntity), true);
            using var transactionScope = await dbContext.Database.BeginTransactionAsync();
            var userName = userEntity.UserName;
            UserType existingUser = await userManager.FindByNameAsync(userName);

            if (!(existingUser is null)) {
                throw new UserAlreadyCreatedException(userName, $"The user '{userName}' has been already created.");
            }
            // User does not exist, we continue user creation.
            else {
                var insensitiveErrorMessage = $"The user '{userName}' could not be created.";

                try {
                    var userResult = await userManager.CreateAsync(userEntity, password);

                    if (!userResult.Succeeded) {
                        var aggregatedError = userResult.ToKeyAggregateException(insensitiveErrorMessage);
                        throw aggregatedError;
                    }
                } catch (Exception error) {
                    logger.LogCritical(error, insensitiveErrorMessage);
                    throw;
                }
            }

            var loadedUser = await GetUserByNameAsync(userName);
            //dbContext.Entrie(loadedUser).State = EntityState.Detached;

            if (roles != null) {
                foreach (var roleName in roles) {
                    var roleAssignmentErrorMessage = $"The user '{userName}' could not be assigned to role '{roleName}'. The user creation has been aborted.";

                    try {
                        if (string.IsNullOrWhiteSpace(roleName)) {
                            var errorMessage = $"The role '{userName}' must not be null or empty.";
                            throw new ArgumentException(errorMessage);
                        }

                        if (!await userManager.IsInRoleAsync(loadedUser, roleName)) {
                            var result = await userManager.AddToRoleAsync(loadedUser, roleName);

                            if (!result.Succeeded) {
                                var aggregatedError = result.ToKeyAggregateException(roleAssignmentErrorMessage);
                                throw aggregatedError;
                            }
                        }
                    } catch (Exception error) {
                        logger.LogCritical(error, roleAssignmentErrorMessage);
                        throw;
                    }
                }
            }

            await transactionScope.CommitAsync();
            return loadedUser;
        }

        public async Task InvalidateSecurityStampAsync(string userId)
        {
            try {
                var userEntity = await userManager.FindByNameAsync(userId);

                if (userEntity == null) {
                    throw new ArgumentException($"The user by id '{userId}' does not exist.");
                }

                var result = await userManager.UpdateSecurityStampAsync(userEntity);

                if (!result.Succeeded) {
                    throw result.ToKeyAggregateException("Invalidating the security stamp failed.");
                }
            } catch (Exception error) {
                var insensitiveErrorMessage = "Invalidating the security stamp failed.";
                logger.LogError(error, insensitiveErrorMessage);
                throw;
            }
        }
    }
}
