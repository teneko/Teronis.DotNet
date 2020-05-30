using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity.Authentication
{
    public class JwtBearerAuthenticationOptions
    {
        public bool IncludeErrorDetails { get; set; }
        [Required]
        public SymmetricSecurityKey TokenSigningKey { get; }

        public JwtBearerAuthenticationOptions(SymmetricSecurityKey tokenSigningKey)
        {
            TokenSigningKey = tokenSigningKey;
        }
    }
}
