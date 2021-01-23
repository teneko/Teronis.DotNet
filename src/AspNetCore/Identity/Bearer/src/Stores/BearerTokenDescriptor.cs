using Microsoft.IdentityModel.Tokens;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
{
    public class BearerTokenDescriptor : SecurityTokenDescriptor
    {
        public new SigningCredentials? SigningCredentials {
            get => base.SigningCredentials;
        }

        public BearerTokenDescriptor(SigningCredentials? signingCredentials) =>
            base.SigningCredentials = signingCredentials;
    }
}
