using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity.Extensions
{
    public static class SecurityTokenDescriptorExtensions
    {
        /// <summary>
        /// Move claims (<see cref="SecurityTokenDescriptor.Claims"/>) to 
        /// <see cref="SecurityTokenDescriptor.Subject"/>. 
        /// Duplicates are ignored.
        /// </summary>
        public static void MoveClaimsToSubjectClaims(this SecurityTokenDescriptor tokenDescriptor)
        {
            if (ReferenceEquals(tokenDescriptor, null)) {
                return;
            }

            var claims = tokenDescriptor.Claims?.Select(x => new Claim(x.Key, x.Value.ToString()));
            // Cache it because it will get overridden.
            var subject = tokenDescriptor.Subject;

            if (ReferenceEquals(subject, null) && ReferenceEquals(claims, null)) {
                // We can skip moving due to no data to move.
                return;
            }

            tokenDescriptor.Subject = new ClaimsIdentity(
                claims,
                subject?.AuthenticationType,
                subject?.NameClaimType,
                subject?.RoleClaimType);

            if (!ReferenceEquals(subject, null)) {
                /// Add missing claims. (see comment in <see cref="SecurityTokenDescriptor.Claims"/>)
                tokenDescriptor.Subject.AddClaims(subject.Claims);
            }

            if (!ReferenceEquals(tokenDescriptor.Claims, null)) {
                tokenDescriptor.Claims.Clear();
            }
        }
    }
}
