using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity.Authentication
{
    public class JwtBearerAuthenticationOptions
    {
        [Required]
        public SymmetricSecurityKey TokenSigningKey { get; set; }

        public JwtBearerAuthenticationOptions(SymmetricSecurityKey tokenSigningKey)
        {
            TokenSigningKey = tokenSigningKey;
        }
    }
}
