using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity
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
