using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.Identity.Presenters.Extensions;
using Teronis.Identity.Entities;
using Teronis.Identity.Extensions;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public abstract class BearerSignInManager<UserType, BearerTokenType> : IBearerSignInManager 
        where UserType : class, IUserEntity
        where BearerTokenType : class, IBearerTokenEntity
    {
        private readonly BearerSignInManagerOptions signInServiceOptions;
        private readonly UserManager<UserType> userManager;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly IBearerTokenStore<BearerTokenType> bearerTokenStore;
        private readonly ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger;

        public BearerSignInManager(IOptions<BearerSignInManagerOptions> options,
            UserManager<UserType> userManager, IOptions<IdentityOptions> identityOptions,
            IBearerTokenStore<BearerTokenType> bearerTokenStore, ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger = null)
        {
            signInServiceOptions = options.Value;
            this.userManager = userManager;
            this.identityOptions = identityOptions;
            this.bearerTokenStore = bearerTokenStore;
            this.logger = logger;
        }

        protected abstract BearerTokenType CreateRefreshToken(string userId, DateTime issuedAtUtc, DateTime expiresAtUtc);

        /// <summary>
        /// Set context user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown by <see cref="UserManager{TUser}.GetUserAsync(ClaimsPrincipal)"/>.
        /// </exception>
        protected async Task<bool> TrySetContextUserAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            try {
                var user = await userManager.GetUserAsync(context.Principal);

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
        protected virtual async Task<bool> TryDeleteExpiredRefreshTokensAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var user = context.User ?? throw BearerSignInManagerThrowHelper.GetContextArgumentException(nameof(context.User));

            try {
                await bearerTokenStore.DeleteExpiredOnesAsync();
                return true;
            } catch (Exception error) {
                logger?.LogError(error, "Expired refresh tokens could not be deleted");
            }

            return false;
        }

        /// <summary>
        /// Deletes the refresh token associated by <see cref="BearerSignInManagerContext.Principal"/>.
        /// </summary>
        protected async Task<bool> TryDeleteUserRefreshTokenAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var findRefreshTokenIdResult = BearerSignInManagerTools.FindRefreshTokenId(context.Principal);

            if (findRefreshTokenIdResult.Succeeded)
                try {
                    await TryDeleteExpiredRefreshTokensAsync(context);

                    if (await bearerTokenStore.TryDeleteAsync(findRefreshTokenIdResult.Content)) {
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
        protected virtual async Task<bool> TrySetContextAccessTokenAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var user = context.User ?? throw BearerSignInManagerThrowHelper.GetContextArgumentException(nameof(context.User));
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

                context.AccessToken = BearerSignInManagerTools.GenerateJwtToken(accessTokenDescriptor, signInServiceOptions.SetDefaultTimesOnTokenCreation);
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

        protected async Task<bool> TryStoreRefreshTokenEntityAsync(BearerSignInManagerContext<UserType, BearerTokenType> context, BearerTokenType refreshToken)
        {
            refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));

            try {
                await bearerTokenStore.InsertAsync(refreshToken);
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
        protected virtual async Task<bool> TrySetContextRefreshTokenEntityAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var user = context.User ?? throw new ArgumentNullException(nameof(BearerSignInManagerContext<UserType, BearerTokenType>.User));
            var refreshTokenDescriptor = signInServiceOptions.CreateRefreshTokenDescriptor();

            var issuedAtUtc = refreshTokenDescriptor.IssuedAt == null ? DateTime.UtcNow :
                DateTime.SpecifyKind((DateTime)refreshTokenDescriptor.IssuedAt, DateTimeKind.Utc);

            var expiresAtUtc = refreshTokenDescriptor.Expires ?? throw new ArgumentNullException(nameof(refreshTokenDescriptor.Expires));
            var refreshTokenEntity = CreateRefreshToken(user.Id, issuedAtUtc, expiresAtUtc);
            var hasStoringSucceeded = await TryStoreRefreshTokenEntityAsync(context, refreshTokenEntity);

            if (hasStoringSucceeded) {
                refreshTokenDescriptor.Claims.Add(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp);
                refreshTokenDescriptor.Claims.Add(BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType, refreshTokenEntity.BearerTokenId);
                var refreshToken = BearerSignInManagerTools.GenerateJwtToken(refreshTokenDescriptor, signInServiceOptions.SetDefaultTimesOnTokenCreation);
                context.RefreshTokenEntity = refreshTokenEntity;
                context.RefreshToken = refreshToken;
                return true;
            }

            return false;
        }

        protected virtual async Task<bool> TrySetContextSignInTokensAsync(BearerSignInManagerContext<UserType, BearerTokenType> context) =>
            await TrySetContextAccessTokenAsync(context) &&
            await TrySetContextRefreshTokenEntityAsync(context);

        /// <summary>
        /// Create next sign in tokens, so a new access token.
        /// </summary>
        public async Task<IServiceResult<SignInTokens>> CreateNextSignInTokensAsync(ClaimsPrincipal principal)
        {
            principal = principal ?? throw BearerSignInManagerThrowHelper.GetPrincipalNullException(nameof(principal));
            var context = new BearerSignInManagerContext<UserType, BearerTokenType>(principal);

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
            var context = new BearerSignInManagerContext<UserType, BearerTokenType>(principal);
            var hasRefreshTokenId = Guid.TryParse(principal.FindFirstValue(BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType), out _);

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
