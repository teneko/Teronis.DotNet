using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Teronis.Identity.Presenters.Extensions;
using Teronis.Identity.Entities;
using Teronis.Identity.Extensions;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.SignInServicing
{
    public class SignInService
    {
        private readonly SignInServiceOptions signInServiceOptions;
        private readonly IdentityContextBase identityContext;
        private readonly UserManager<UserEntity> userManager;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly ILogger<SignInService>? logger;

        public SignInService(IOptions<SignInServiceOptions> options, IdentityContextBase dbContext,
            UserManager<UserEntity> userManager, IOptions<IdentityOptions> identityOptions,
            ILogger<SignInService>? logger = null)
        {
            signInServiceOptions = options.Value;
            identityContext = dbContext;
            this.userManager = userManager;
            this.identityOptions = identityOptions;
            this.logger = logger;
        }


        public async Task<ServiceResult<RefreshTokenEntity>> FindRefreshTokenAsync(ClaimsPrincipal principal)
        {
            principal = principal ?? throw SignInServiceThrowHelper.GetPrincipalNullException(nameof(principal));
            var refreshTokenIdResult = SignInServiceTools.FindRefreshTokenId(principal);

            if (!refreshTokenIdResult.Succeeded) {
                return ServiceResult<RefreshTokenEntity>.Failed(refreshTokenIdResult);
            }

            try {
                // Then we need the entity that belongs to refresh token id.
                var refreshTokenEntity = await identityContext.RefreshTokens.FindAsync(refreshTokenIdResult.Content);

                return ReferenceEquals(refreshTokenEntity, null) ?
                    ServiceResult<RefreshTokenEntity>
                        .FailedWithErrorMessage("The refresh token has been redeemed")
                        .WithHttpStatusCode(HttpStatusCode.BadRequest) :
                    ServiceResult<RefreshTokenEntity>
                        .SucceededWithContent(refreshTokenEntity)
                        .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                const string errorMessage = "Search for refresh token failed";
                logger?.LogError(error, errorMessage);

                return errorMessage.ToJsonError()
                    .ToServiceResultFactory<RefreshTokenEntity>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError)
                    .AsServiceResult();
            }
        }

        /// <summary>
        /// Set context user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown by <see cref="UserManager{TUser}.GetUserAsync(ClaimsPrincipal)"/>.
        /// </exception>
        protected async Task<bool> TrySetContextUserAsync(SignInServiceContext context)
        {
            try {
                var user = await userManager.GetUserAsync(context.Principal);

                await identityContext.Entry(user)
                    .Collection(x => x.RefreshTokens)
                    .LoadAsync();

                if (!ReferenceEquals(user, null)) {
                    context.User = user;
                    return true;
                } else {
                    const string errorMessage = "The user couldn't be found";
                    logger?.LogDebug(errorMessage);

                    context.SetResult()
                        .ToFailedWithErrorMessage(errorMessage)
                        .WithHttpStatusCode(HttpStatusCode.BadRequest);
                }
            } catch (Exception error) {
                logger?.LogError(error, $"The user couldn't be loaded");

                context.SetResult()
                    .ToFailed()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
            }

            context.User = null;
            return false;
        }

        /// <summary>
        /// Deletes expired refresh tokens.
        /// </summary>
        public virtual async Task<bool> TryDeleteExpiredRefreshTokensAsync(SignInServiceContext context)
        {
            var user = context.User ?? throw SignInServiceThrowHelper.GetContextArgumentException(nameof(context.User));

            try {
                var expiredRefreshTokens = await identityContext.RefreshTokens
                    .AsAsyncEnumerable()
                    .Where(x => x.UserId == user.Id && DateTime.UtcNow > x.ExpiresAtUtc)
                    .ToListAsync();

                identityContext.RefreshTokens.RemoveRange(expiredRefreshTokens);
                await identityContext.SaveChangesAsync();
                return true;
            } catch (Exception error) {
                logger?.LogError(error, "Expired refresh tokens could not be deleted");
            }

            return false;
        }

        /// <summary>
        /// Deletes the refresh token associated by <see cref="SignInServiceContext.Principal"/>.
        /// </summary>
        protected async Task<bool> TryDeleteUserRefreshTokenAsync(SignInServiceContext context)
        {
            var user = context.User ?? throw SignInServiceThrowHelper.GetContextArgumentException(nameof(context.User));
            var findRefreshTokenIdResult = SignInServiceTools.FindRefreshTokenId(context.Principal);

            if (findRefreshTokenIdResult.Succeeded)
                try {
                    await TryDeleteExpiredRefreshTokensAsync(context);

                    if (user.TryDettachRefreshToken(findRefreshTokenIdResult.Content)) {
                        await identityContext.SaveChangesAsync();
                        return true;
                    } else {
                        context.SetResult()
                            .ToFailedWithErrorMessage("The user does not have the refresh token")
                            .WithHttpStatusCode(HttpStatusCode.Unauthorized);
                    }
                } catch (Exception error) {
                    logger?.LogError(error, $"The refresh token couldn't be deleted");

                    context.SetResult()
                        .ToFailed()
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError);
                }
            else {
                context.SetResult()
                    .ToFailed(findRefreshTokenIdResult);
            }

            return false;
        }

        /// <summary>
        /// The access token does contain user user id, user name and user roles.
        /// </summary>
        protected virtual async Task<bool> TrySetContextAccessTokenAsync(SignInServiceContext context)
        {
            var user = context.User ?? throw SignInServiceThrowHelper.GetContextArgumentException(nameof(context.User));
            var accessTokenDescriptor = signInServiceOptions.CreateAccessTokenDescriptor();

            // Used by authentication middleware.
            accessTokenDescriptor.Claims.Add(ClaimTypes.NameIdentifier, user.Id);
            accessTokenDescriptor.Claims.Add(ClaimTypes.Name, user.UserName);

            try {
                var roles = await userManager.GetRolesAsync(user);

                if (roles != null) {
                    foreach (var role in roles) {
                        accessTokenDescriptor.Claims.Add(ClaimTypes.Role, role);
                    }
                }

                context.AccessToken = SignInServiceTools.GenerateJwtToken(accessTokenDescriptor, signInServiceOptions.SetDefaultTimesOnTokenCreation);
                return true;
            } catch (Exception error) {
                var errorMessage = $"The access token couldn't be created";
                logger.LogError(error, errorMessage);

                context.SetResult()
                    .ToFailedWithErrorMessage(errorMessage)
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
            }

            return false;
        }

        protected async Task<bool> TryStoreRefreshTokenEntityAsync(SignInServiceContext context, RefreshTokenEntity refreshToken)
        {
            var user = context.User ?? throw SignInServiceThrowHelper.GetContextArgumentException(nameof(context.User));
            refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            user.AttachRefreshToken(refreshToken, false);

            try {
                await identityContext.SaveChangesAsync();
                return true;
            } catch (Exception error) {
                var errorMessage = $"Refresh token cannot be stored. Try again later.";
                logger?.LogError(error, errorMessage);

                context.SetResult()
                    .ToFailedWithErrorMessage(errorMessage)
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized);
            }

            return false;
        }

        /// <summary>
        /// The refresh token does contain user security stamp and refresh token id.
        /// </summary>
        protected virtual async Task<bool> TrySetContextRefreshTokenEntityAsync(SignInServiceContext context)
        {
            var user = context.User ?? throw new ArgumentNullException(nameof(SignInServiceContext.User));
            var refreshTokenDescriptor = signInServiceOptions.CreateRefreshTokenDescriptor();

            var issuedAtUtc = refreshTokenDescriptor.IssuedAt == null ? DateTime.UtcNow :
                DateTime.SpecifyKind((DateTime)refreshTokenDescriptor.IssuedAt, DateTimeKind.Utc);

            var expiresAtUtc = refreshTokenDescriptor.Expires ?? throw new ArgumentNullException(nameof(refreshTokenDescriptor.Expires));
            var refreshTokenEntity = new RefreshTokenEntity(user.Id, issuedAtUtc, expiresAtUtc);
            var hasStoringSucceeded = await TryStoreRefreshTokenEntityAsync(context, refreshTokenEntity);

            if (hasStoringSucceeded) {
                refreshTokenDescriptor.Claims.Add(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp);
                refreshTokenDescriptor.Claims.Add(SignInServiceDefaults.SignInServiceRefreshTokenIdClaimType, refreshTokenEntity.RefreshTokenId);
                var refreshToken = SignInServiceTools.GenerateJwtToken(refreshTokenDescriptor, signInServiceOptions.SetDefaultTimesOnTokenCreation);
                context.RefreshTokenEntity = refreshTokenEntity;
                context.RefreshToken = refreshToken;
                return true;
            }

            return false;
        }

        protected virtual async Task<bool> TrySetContextSignInTokensAsync(SignInServiceContext context) =>
            await TrySetContextAccessTokenAsync(context) &&
            await TrySetContextRefreshTokenEntityAsync(context);

        /// <summary>
        /// Create next sign in tokens, so a new access token.
        /// </summary>
        public async Task<IServiceResult<SignInTokens>> CreateNextSignInTokensAsync(ClaimsPrincipal principal)
        {
            principal = principal ?? throw SignInServiceThrowHelper.GetPrincipalNullException(nameof(principal));
            var context = new SignInServiceContext(principal);

            if (await TrySetContextUserAsync(context) && await TrySetContextSignInTokensAsync(context)) {
                var authenticationTokens = new SignInTokens(context.AccessToken!, context.RefreshToken!);

                context.SetResult()
                    .ToSucceededWithContent(authenticationTokens)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            }

            return context.Result!;
        }

        /// <summary>
        /// Create sign in tokens, so a new refresh token and a new access token.
        /// </summary>
        public async Task<IServiceResult<SignInTokens>> CreateInitialSignInTokensAsync(ClaimsPrincipal principal)
        {
            var context = new SignInServiceContext(principal);
            var hasRefreshTokenId = Guid.TryParse(principal.FindFirstValue(SignInServiceDefaults.SignInServiceRefreshTokenIdClaimType), out _);

            if (hasRefreshTokenId) {
                try {
                    if (await TrySetContextUserAsync(context) && await TryDeleteUserRefreshTokenAsync(context) && await TrySetContextSignInTokensAsync(context)) {
                        var refreshTokens = new SignInTokens(context.AccessToken!, context.RefreshToken!);

                        context.SetResult()
                            .ToSucceededWithContent(refreshTokens)
                            .WithHttpStatusCode(HttpStatusCode.OK);
                    }
                } catch (Exception error) {
                    var errorMessage = $"An unhandled exception has been occured";
                    logger.LogError(error, errorMessage);

                    context.SetResult()
                        .ToFailedWithErrorMessage(errorMessage)
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError);
                }
            } else {
                context.SetResult()
                    .ToFailedWithErrorMessage("The refresh token is not valid")
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized);
            }

            return context.Result!;
        }
    }
}
