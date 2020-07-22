using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.Identity.Entities;
using Teronis.Identity.Extensions;
using Teronis.Mvc.ServiceResulting;
using Teronis.Mvc.ServiceResulting.Extensions;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public abstract class BearerSignInManager<UserType, BearerTokenType> : IBearerSignInManager
        where UserType : class, IBearerUserEntity
        where BearerTokenType : class, IBearerTokenEntity
    {
        private readonly ErrorDetailsProvider errorDetailsProvider;
        private readonly BearerSignInManagerOptions signInManagerOptions;
        private readonly UserManager<UserType> userManager;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly IBearerTokenStore<BearerTokenType> bearerTokenStore;
        private readonly ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger;

        public BearerSignInManager(IOptions<BearerSignInManagerOptions> options,
            UserManager<UserType> userManager, IOptions<IdentityOptions> identityOptions,
            IBearerTokenStore<BearerTokenType> bearerTokenStore, ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger = null)
        {
            errorDetailsProvider = new ErrorDetailsProvider(() => options.Value.IncludeErrorDetails, logger);
            signInManagerOptions = options.Value;
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
                    const string errorMessage = "The user could not be found.";
                    logger?.LogDebug(errorMessage);

                    context.SetResult()
                        .ToFailure(errorMessage)
                        .WithHttpStatusCode(HttpStatusCode.BadRequest);
                }
            } catch (Exception error) {
                var insensitiveErrorMessage = $"The user could not be loaded.";

                context.SetResult(errorDetailsProvider.LogCriticalThenBuildAppropiateError<object>(error, insensitiveErrorMessage)
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError));
            }

            context.User = null;
            return false;
        }

        /// <summary>
        /// Deletes expired refresh tokens.
        /// </summary>
        protected virtual async Task<bool> TryDeleteExpiredRefreshTokensAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            try {
                await bearerTokenStore.DeleteExpiredOnesAsync();
                return true;
            } catch (Exception error) {
                logger?.LogError(error, "Expired refresh tokens could not be deleted.");
            }

            return false;
        }

        /// <summary>
        /// Deletes the refresh token associated by <see cref="BearerSignInManagerContext.Principal"/>.
        /// </summary>
        protected async Task<bool> TryDeleteUserRefreshTokenAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var findRefreshTokenIdResult = BearerSignInManagerUtils.FindRefreshTokenId(context.Principal);

            if (findRefreshTokenIdResult.Succeeded) {
                try {
                    await TryDeleteExpiredRefreshTokensAsync(context);

                    if (await bearerTokenStore.TryDeleteAsync(findRefreshTokenIdResult.Content)) {
                        return true;
                    } else {
                        context.SetResult()
                            .ToFailure("The user does not have the refresh token.")
                            .WithHttpStatusCode(HttpStatusCode.Unauthorized);
                    }
                } catch (Exception? error) {
                    context.SetResult(errorDetailsProvider.LogErrorThenBuildAppropiateError<object>(error, "The refresh token could not be deleted.")
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError));
                }
            } else {
                context.SetResult()
                    .ToFailure(findRefreshTokenIdResult);
            }

            return false;
        }

        /// <summary>
        /// The access token does contain user user id, user name and user roles.
        /// </summary>
        protected virtual async Task<bool> TrySetContextAccessTokenAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var user = context.User ?? throw BearerSignInManagerThrowHelper.GetContextArgumentException(nameof(context.User));
            var accessTokenDescriptor = signInManagerOptions.CreateAccessTokenDescriptor();

            // Used by authentication middleware.
            accessTokenDescriptor.Subject ??= new ClaimsIdentity();
            accessTokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            accessTokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            try {
                var roles = await userManager.GetRolesAsync(user);

                if (roles != null) {
                    foreach (var role in roles) {
                        accessTokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }

                context.AccessToken = BearerSignInManagerUtils.GenerateJwtToken(accessTokenDescriptor, signInManagerOptions.SetDefaultTimesOnTokenCreation);
                return true;
            } catch (Exception error) {
                context.SetResult(errorDetailsProvider.LogCriticalThenBuildAppropiateError<object>(error, "The access token could not be created.")
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError));
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
                context.SetResult(errorDetailsProvider.LogErrorThenBuildAppropiateError<object>(error, "Refresh token cannot be stored. Try again later.")
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized));
            }

            return false;
        }

        /// <summary>
        /// The refresh token does contain user security stamp and refresh token id.
        /// </summary>
        protected virtual async Task<bool> TrySetContextRefreshTokenEntityAsync(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var user = context.User ?? throw new ArgumentNullException(nameof(BearerSignInManagerContext<UserType, BearerTokenType>.User));
            var refreshTokenDescriptor = signInManagerOptions.CreateRefreshTokenDescriptor();

            var issuedAtUtc = refreshTokenDescriptor.IssuedAt == null ? DateTime.UtcNow :
                DateTime.SpecifyKind((DateTime)refreshTokenDescriptor.IssuedAt, DateTimeKind.Utc);

            var expiresAtUtc = refreshTokenDescriptor.Expires ?? throw new ArgumentNullException(nameof(refreshTokenDescriptor.Expires));
            var refreshTokenEntity = CreateRefreshToken(user.Id, issuedAtUtc, expiresAtUtc);
            var hasStorageSucceeded = await TryStoreRefreshTokenEntityAsync(context, refreshTokenEntity);

            if (hasStorageSucceeded) {
                refreshTokenDescriptor.Claims.Add(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp);
                refreshTokenDescriptor.Claims.Add(BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType, refreshTokenEntity.BearerTokenId);
                var refreshToken = BearerSignInManagerUtils.GenerateJwtToken(refreshTokenDescriptor, signInManagerOptions.SetDefaultTimesOnTokenCreation);
                context.RefreshTokenEntity = refreshTokenEntity;
                context.RefreshToken = refreshToken;
                return true;
            }

            return false;
        }

        protected virtual async Task<bool> TrySetContextSignInTokensAsync(BearerSignInManagerContext<UserType, BearerTokenType> context) =>
            await TrySetContextAccessTokenAsync(context) &&
            await TrySetContextRefreshTokenEntityAsync(context);

        /// <inheritdoc/>
        public async Task<IServiceResult<SignInTokens>> CreateTokensAsync(ClaimsPrincipal principal)
        {
            principal = principal ?? throw BearerSignInManagerThrowHelper.GetPrincipalNullException(nameof(principal));
            var context = new BearerSignInManagerContext<UserType, BearerTokenType>(principal);

            if (await TrySetContextUserAsync(context) && await TrySetContextSignInTokensAsync(context)) {
                var authenticationTokens = new SignInTokens(context.AccessToken!, context.RefreshToken!);
                return ServiceResult<SignInTokens>.Success(authenticationTokens).WithHttpStatusCode(HttpStatusCode.OK);
            }

            return context.Result!.CopyButFailed<object, SignInTokens>();
        }

        public bool HasPrincipalRefreshToken(BearerSignInManagerContext<UserType, BearerTokenType> context)
        {
            var principal = context.Principal ?? throw BearerSignInManagerThrowHelper.GetContextArgumentException(nameof(BearerSignInManagerContext<UserType, BearerTokenType>.Principal));
            var hasRefreshTokenId = Guid.TryParse(principal.FindFirstValue(BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType), out _);

            if (!hasRefreshTokenId) {
                context.SetResult()
                    .ToFailure("The refresh token is not valid.")
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized);

                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<IServiceResult<SignInTokens>> CreateNextTokensAsync(ClaimsPrincipal principal)
        {
            var context = new BearerSignInManagerContext<UserType, BearerTokenType>(principal);

            if (HasPrincipalRefreshToken(context)) {
                try {
                    if (await TrySetContextUserAsync(context) && await TryDeleteUserRefreshTokenAsync(context) && await TrySetContextSignInTokensAsync(context)) {
                        var refreshTokens = new SignInTokens(context.AccessToken!, context.RefreshToken!);
                        return ServiceResult<SignInTokens>.Success(refreshTokens).WithHttpStatusCode(HttpStatusCode.OK);
                    }
                } catch (Exception error) {
                    context.SetResult(errorDetailsProvider.LogErrorThenBuildAppropiateError<object>(error)
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError));
                }
            }

            return context.Result!.CopyButFailed<object, SignInTokens>();
        }

        public async Task<IServiceResult> InvalidateIssuesAsync(ClaimsPrincipal principal)
        {
            var context = new BearerSignInManagerContext<UserType, BearerTokenType>(principal);

            if (HasPrincipalRefreshToken(context)) {
                try {
                    if (await TrySetContextUserAsync(context) && await TryDeleteUserRefreshTokenAsync(context)) {
                        return new ServiceResult(true).WithHttpStatusCode(HttpStatusCode.OK);
                    }
                } catch (Exception error) {
                    context.SetResult(errorDetailsProvider.LogErrorThenBuildAppropiateError<object>(error)
                        .WithHttpStatusCode(HttpStatusCode.InternalServerError));
                }
            }

            return context.Result!.CopyButFailed<object, object>();
        }
    }
}
