using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.Identity.Bearer.SignInManaging;
using Teronis.Identity.Bearer.Stores;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Bearer
{
    public abstract class BearerSignInManager<UserType, BearerTokenType> : IBearerSignInManager
        where UserType : class, IBearerUserEntity
        where BearerTokenType : class, IBearerTokenEntity
    {
        private readonly BearerSignInManagerOptions signInManagerOptions;
        private readonly UserManager<UserType> userManager;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly IBearerTokenStore<BearerTokenType> bearerTokenStore;
        private readonly ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger;

        public BearerSignInManager(IOptions<BearerSignInManagerOptions> options,
            UserManager<UserType> userManager, IOptions<IdentityOptions> identityOptions,
            IBearerTokenStore<BearerTokenType> bearerTokenStore, ILogger<BearerSignInManager<UserType, BearerTokenType>>? logger = null)
        {
            //errorDetailsProvider = new ServiceErrorResultDetailsProvider(() => options.Value.IncludeErrorDetails, logger);
            signInManagerOptions = options.Value;
            this.userManager = userManager;
            this.identityOptions = identityOptions;
            this.bearerTokenStore = bearerTokenStore;
            this.logger = logger;
        }

        protected abstract BearerTokenType CreateRefreshToken(string userId, DateTime issuedAtUtc, DateTime expiresAtUtc);

        /// <summary>
        /// Gets user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown by <see cref="UserManager{TUser}.GetUserAsync(ClaimsPrincipal)"/>.
        /// </exception>
        protected async Task<UserType> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);

            if (user is null) {
                const string errorMessage = "The user could not be found.";
                logger?.LogDebug(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            return user;
        }

        /// <summary>
        /// Deletes expired refresh tokens.
        /// </summary>
        protected virtual async Task<bool> TryDeleteExpiredRefreshTokensAsync()
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
        protected async Task DeleteUserRefreshTokenAsync(ClaimsPrincipal principal)
        {
            var refreshTokenId = BearerSignInManagerUtils.FindRefreshTokenId(principal);

            try {
                // Try but dont rely on expire refreshed tokens.
                await TryDeleteExpiredRefreshTokensAsync();

                if (!await bearerTokenStore.TryDeleteAsync(refreshTokenId)) {
                    throw new ArgumentException("The refresh token cannot be deleted because its identifier is not deposited.");
                }
            } catch (Exception? error) {
                var insensitiveErrorMessage = "The refresh token could not be deleted.";
                logger.LogCritical(error, insensitiveErrorMessage);
                throw;
            }
        }

        /// <summary>
        /// The access token does contain user user id, user name and user roles.
        /// </summary>
        protected virtual async Task<string> GenerateAccessTokenAsync(UserType user)
        {
            user = user ?? throw new ArgumentNullException(nameof(user));
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

                return BearerSignInManagerUtils.GenerateJwtToken(accessTokenDescriptor, signInManagerOptions.SetDefaultTimesOnTokenCreation);
            } catch (Exception error) {
                var insensitiveErrorMessage = "The access token could not be created.";
                logger.LogCritical(error, insensitiveErrorMessage);
                throw;
            }
        }

        protected async Task StoreRefreshTokenEntityAsync(BearerTokenType refreshToken)
        {
            refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));

            try {
                await bearerTokenStore.InsertAsync(refreshToken);
            } catch (Exception error) {
                var insensitiveErrorMessage = "The refresh token could not be stored.";
                logger.LogCritical(error, insensitiveErrorMessage);
                throw;
            }
        }

        /// <summary>
        /// The refresh token does contain user security stamp and refresh token id.
        /// </summary>
        protected virtual async Task<(BearerTokenType RefreshTokenEntity, string RefreshToken)> GenerateAndStoreRefreshTokenEntityAsync(UserType user)
        {
            user = user ?? throw new ArgumentNullException(nameof(user));
            var refreshTokenDescriptor = signInManagerOptions.CreateRefreshTokenDescriptor();

            var issuedAtUtc = refreshTokenDescriptor.IssuedAt == null ? DateTime.UtcNow :
                DateTime.SpecifyKind((DateTime)refreshTokenDescriptor.IssuedAt, DateTimeKind.Utc);

            var expiresAtUtc = refreshTokenDescriptor.Expires ?? throw new ArgumentNullException(nameof(refreshTokenDescriptor.Expires));
            var refreshTokenEntity = CreateRefreshToken(user.Id, issuedAtUtc, expiresAtUtc);
            await StoreRefreshTokenEntityAsync(refreshTokenEntity);
            refreshTokenDescriptor.Claims.Add(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp);
            refreshTokenDescriptor.Claims.Add(BearerSignInManagerDefaults.BearerSignInManagerRefreshTokenIdClaimType, refreshTokenEntity.BearerTokenId);
            var refreshToken = BearerSignInManagerUtils.GenerateJwtToken(refreshTokenDescriptor, signInManagerOptions.SetDefaultTimesOnTokenCreation);
            return (RefreshTokenEntity: refreshTokenEntity, RefreshToken: refreshToken);
        }

        protected virtual async Task<(string AccessToken, BearerTokenType RefreshTokenEntity, string RefreshToken)> GenerateAndStoreSignInTokensAsync(UserType user)
        {
            var accesssToken = await GenerateAccessTokenAsync(user);
            var (refreshTokenEntity, refreshToken) = await GenerateAndStoreRefreshTokenEntityAsync(user);
            return (AccessToken: accesssToken, RefreshTokenEntity: refreshTokenEntity, RefreshToken: refreshToken);
        }

        /// <inheritdoc/>
        public async Task<SignInTokens> CreateTokensAsync(ClaimsPrincipal principal)
        {
            principal = principal ?? throw new ArgumentNullException(nameof(principal));
            var user = await GetUserAsync(principal);
            var signInTokens = await GenerateAndStoreSignInTokensAsync(user);
            var authenticationTokens = new SignInTokens(signInTokens.AccessToken, signInTokens.RefreshToken);
            return authenticationTokens;
        }

        /// <summary>
        /// Validates whether the refresh token is valid.
        /// </summary>
        /// <param name="principal"></param>
        /// <exception cref="FormatException" />
        public void ValidatePrincipalRefreshToken(ClaimsPrincipal principal)
        {
            principal = principal ?? throw new ArgumentNullException(nameof(principal));
            var hasRefreshTokenId = Guid.TryParse(principal.FindFirstValue(BearerSignInManagerDefaults.BearerSignInManagerRefreshTokenIdClaimType), out _);

            if (!hasRefreshTokenId) {
                throw new FormatException("The refresh token is not valid.");
            }
        }

        /// <inheritdoc/>
        public async Task<SignInTokens> CreateNextTokensAsync(ClaimsPrincipal principal)
        {
            ValidatePrincipalRefreshToken(principal);
            var user = await GetUserAsync(principal);
            await DeleteUserRefreshTokenAsync(principal);
            var signInTokensTuple = await GenerateAndStoreSignInTokensAsync(user);
            var signInTokens = new SignInTokens(signInTokensTuple.AccessToken, signInTokensTuple.RefreshToken);
            return signInTokens;
        }

        public async Task InvalidateRefreshTokenAsync(ClaimsPrincipal principal)
        {
            ValidatePrincipalRefreshToken(principal);
            await DeleteUserRefreshTokenAsync(principal);
        }
    }
}
