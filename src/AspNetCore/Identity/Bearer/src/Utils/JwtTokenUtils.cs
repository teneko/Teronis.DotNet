// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Teronis.AspNetCore.Identity.Extensions;

namespace Teronis.AspNetCore.Identity.Bearer.Utils
{
    public static class JwtTokenUtils
    {
        /// <summary>
        /// Generates a JWT-token from token descriptor.
        /// </summary>
        public static string GenerateJwtToken(SecurityTokenDescriptor securityTokenDescriptor, bool setDefaultTimesOnTokenCreation)
        {
            // We want to move claims to the claims of subject.
            securityTokenDescriptor.MoveClaimsToSubjectClaims();
            var tokenHandler = new JwtSecurityTokenHandler() { SetDefaultTimesOnTokenCreation = setDefaultTimesOnTokenCreation };
            // Create security token.
            var securityToken = tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            // Generate token string.
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
