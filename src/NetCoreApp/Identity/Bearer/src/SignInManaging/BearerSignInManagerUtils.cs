using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Teronis.Identity.Bearer.SignInManaging;
using Teronis.Identity.Bearer.Stores;
using Teronis.Identity.Bearer.Utils;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Bearer
{
    public static class BearerSignInManagerUtils
    {
        /// <summary>
        /// It looks for claim <see cref="BearerSignInManagerDefaults.BearerSignInManagerRefreshTokenIdClaimType"/>.
        /// </summary>
        /// <exception cref="ArgumentException" />
        /// <exception cref="FormatException" />
        public static Guid FindRefreshTokenId(ClaimsPrincipal principal)
        {
            var refreshTokenIdString = principal.FindFirstValue(BearerSignInManagerDefaults.BearerSignInManagerRefreshTokenIdClaimType);

            if (string.IsNullOrEmpty(refreshTokenIdString)) {
                throw new ArgumentException("The refresh token id must not be null or empty.");
            } else if (!Guid.TryParse(refreshTokenIdString, out var refreshTokenId)) {
                throw new FormatException("The refresh token id is not in correct format.");
            } else {
                return refreshTokenId;
            }
        }

        /// <summary>
        /// It tries to resolve refresh token id from claim <see cref="BearerSignInManagerDefaults.BearerSignInManagerRefreshTokenIdClaimType"/> 
        /// and then look in the database. If a refresh token has been found, it will be returned.
        /// </summary>
        /// <exception cref="BearerSignInException" />
        public static async Task<BearerTokenType> FindRefreshTokenAsync<BearerTokenType>(IBearerTokenStore<BearerTokenType> refreshTokenStore, ClaimsPrincipal principal, ILogger? logger = null)
            where BearerTokenType : class, IBearerTokenEntity
        {
            principal = principal ?? throw new ArgumentNullException(nameof(principal));
            var refreshTokenId = FindRefreshTokenId(principal);
            // Then we need the entity that belongs to refresh token id.
            var refreshTokenEntity = await refreshTokenStore.FindAsync(refreshTokenId);
            return refreshTokenEntity;
        }

        /// <summary>
        /// Generates a service concrete JWT-token from token descriptor.
        /// </summary>
        public static string GenerateJwtToken(SecurityTokenDescriptor tokenDescriptor, bool setDefaultTimesOnTokenCreation)
        {
            tokenDescriptor = tokenDescriptor ?? throw new ArgumentNullException(nameof(tokenDescriptor));
            tokenDescriptor.Claims ??= new Dictionary<string, object>();
            return JwtTokenUtils.GenerateJwtToken(tokenDescriptor, setDefaultTimesOnTokenCreation);
        }
    }
}
