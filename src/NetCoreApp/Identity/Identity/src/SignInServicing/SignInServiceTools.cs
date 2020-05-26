using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Teronis.Identity.Extensions;
using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Tools;

namespace Teronis.Identity.SignInServicing
{
    public static class SignInServiceTools
    {
        public static ServiceResult<Guid> FindRefreshTokenId(ClaimsPrincipal principal)
        {
            var refreshTokenIdString = principal.FindFirstValue(SignInServiceDefaults.SignInServiceRefreshTokenIdClaimType);

            if (string.IsNullOrEmpty(refreshTokenIdString)) {
                return "There has been no refresh token id been found."
                    .ToJsonError()
                    .ToServiceResultFactory<Guid>()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .AsServiceResult();
            } else if (!Guid.TryParse(refreshTokenIdString, out var refreshTokenId)) {
                return "The refresh token id is not valid."
                    .ToJsonError()
                    .ToServiceResultFactory<Guid>()
                    .WithHttpStatusCode(HttpStatusCode.Unauthorized)
                    .AsServiceResult();
            } else {
                return ServiceResult<Guid>.SucceededWithContent(refreshTokenId)
                    .WithHttpStatusCode(HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Generates a service concrete JWT-token from token descriptor.
        /// </summary>
        public static string GenerateJwtToken(SecurityTokenDescriptor tokenDescriptor, bool setDefaultTimesOnTokenCreation)
        {
            tokenDescriptor = tokenDescriptor ?? throw new ArgumentNullException(nameof(tokenDescriptor));
            tokenDescriptor.Claims ??= new Dictionary<string, object>();
            return JwtTokenTools.GenerateJwtToken(tokenDescriptor, setDefaultTimesOnTokenCreation);
        }
    }
}
