// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.AspNetCore.Identity.Bearer.Authentication
{
    public class JwtBearerAuthenticationOptions
    {
        public bool IncludeErrorDetails { get; set; }
        [Required]
        public SecurityKey TokenSigningKey { get; }

        public JwtBearerAuthenticationOptions(SecurityKey tokenSigningKey)
        {
            TokenSigningKey = tokenSigningKey;
        }
    }
}
