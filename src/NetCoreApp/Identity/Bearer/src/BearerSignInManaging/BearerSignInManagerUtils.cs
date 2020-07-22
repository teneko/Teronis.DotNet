using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Teronis.Identity.Entities;
using Teronis.Identity.Extensions;
using Teronis.Identity.Utils;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public static class BearerSignInManagerUtils
    {
        /// <summary>
        /// It looks for claim <see cref="BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType"/>.
        /// </summary>
        public static ServiceResult<Guid> FindRefreshTokenId(ClaimsPrincipal principal)
        {
            var refreshTokenIdString = principal.FindFirstValue(BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType);

            if (string.IsNullOrEmpty(refreshTokenIdString)) {
                return "There has been no refresh token id been found."
                    .ToJsonError()
                    .ToServiceResult<Guid>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest);
            } else if (!Guid.TryParse(refreshTokenIdString, out var refreshTokenId)) {
                return "The refresh token id is not valid."
                    .ToJsonError()
                    .ToServiceResult<Guid>()
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized);
            } else {
                return ServiceResult<Guid>.Success(refreshTokenId)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// It tries to resolve refresh token id from claim <see cref="BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType"/> 
        /// and then look in the database. If a refresh token has been found, it will be returned.
        /// </summary>
        public static async Task<ServiceResult<BearerTokenType>> FindRefreshTokenAsync<BearerTokenType>(IBearerTokenStore<BearerTokenType> refreshTokenStore, ClaimsPrincipal principal, ILogger? logger = null)
            where BearerTokenType : class, IBearerTokenEntity
        {
            principal = principal ?? throw BearerSignInManagerThrowHelper.GetPrincipalNullException(nameof(principal));
            var refreshTokenIdResult = FindRefreshTokenId(principal);

            if (!refreshTokenIdResult.Succeeded) {
                return ServiceResult<BearerTokenType>.Failure(refreshTokenIdResult);
            }

            try {
                // Then we need the entity that belongs to refresh token id.
                var refreshTokenEntity = await refreshTokenStore.FindAsync(refreshTokenIdResult.Content);

                return ReferenceEquals(refreshTokenEntity, null) ?
                    ServiceResult<BearerTokenType>
                        .Failure("The refresh token has been redeemed.")
                        .WithHttpStatusCode(HttpStatusCode.BadRequest) :
                    ServiceResult<BearerTokenType>
                        .Success(refreshTokenEntity)
                        .WithHttpStatusCode(HttpStatusCode.OK);
            } catch (Exception error) {
                const string errorMessage = "Search for refresh token failed.";
                logger?.LogError(error, errorMessage);

                return errorMessage.ToJsonError()
                    .ToServiceResult<BearerTokenType>()
                    .WithHttpStatusCode(HttpStatusCode.InternalServerError);
            }
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
