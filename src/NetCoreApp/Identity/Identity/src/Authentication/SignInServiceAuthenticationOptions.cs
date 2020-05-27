using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity.Authentication
{
    public class SignInServiceAuthenticationOptions
    {
        [Required]
        public SymmetricSecurityKey TokenSigningKey { get; set; }

        public SignInServiceAuthenticationOptions(SymmetricSecurityKey tokenSigningKey)
        {
            TokenSigningKey = tokenSigningKey;
        }
    }
}
