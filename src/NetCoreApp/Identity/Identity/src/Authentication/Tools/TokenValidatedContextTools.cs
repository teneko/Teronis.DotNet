using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Identity.BearerSignInManaging;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Authentication.Tools
{
    public static class TokenValidatedContextTools
    {
        /// <summary>
        /// Validates whether <see cref="BearerSignInManagerDefaults.SignInServiceRefreshTokenIdClaimType"/> does exist
        /// and if so, it does add an user related identity to the claims principal of <paramref name="tokenValidatedContext"/>.
        /// </summary>
        public static async Task ValidateRefreshTokenIdClaim<BearerTokenType>(TokenValidatedContext tokenValidatedContext)
            where BearerTokenType : class, IBearerTokenEntity
        {
            var bearerTokenStore = tokenValidatedContext.HttpContext.RequestServices.GetService<IBearerTokenStore<BearerTokenType>>();
            var identityOptions = tokenValidatedContext.HttpContext.RequestServices.GetService<IOptions<IdentityOptions>>();

            var principal = tokenValidatedContext.Principal;
            var result = await BearerSignInManagerTools.FindRefreshTokenAsync(bearerTokenStore, principal);

            if (result.Succeeded) {
                // When succeeded, we can assure that refresh token entity is not null.
                var refreshTokenEntity = result.Content ?? throw new ArgumentException($"The member '{nameof(result.Content)}' is null.");
                var claims = new[] { new Claim(identityOptions.Value.ClaimsIdentity.UserIdClaimType, refreshTokenEntity.UserId.ToString()) };
                var identity = new ClaimsIdentity(claims);
                // We add a user related identity to the claims principal.
                tokenValidatedContext.Principal.AddIdentity(identity);
            } else {
                tokenValidatedContext.Fail(result.ToString());
            }
        }

        /// <summary>
        /// The user may be authenticated but the security stamp could
        /// have been changed and therefore not be authenticated anymore.
        /// In the claims principal of <paramref name="context"/> we rely on
        /// <see cref="ClaimsIdentityOptions.UserIdClaimType"/> and
        /// <see cref="ClaimsIdentityOptions.SecurityStampClaimType"/>.
        /// Therefore it should be in need of, that
        /// <see cref="ValidateRefreshTokenIdClaim(TokenValidatedContext)"/>
        /// is called first.
        /// </summary>
        public static async Task ValidateSecurityStamp(TokenValidatedContext context)
        {
            var signInManager = context.HttpContext.RequestServices.GetService<SignInManager<UserEntity>>();
            // Why does this works?? IdentityOptions.ClaimIdentity.UserIdClaimType is by default
            // ClaimTypes.NameIdentifier, and this one will be grabbed, if not user will be null.
            var user = await signInManager.ValidateSecurityStampAsync(context.Principal);

            /// The result of <see cref="TokenValidatedContext.Result"/> will be set.
            /// And when it is faulty, then this result will be the http response.
            if (user == null) {
                context.Fail("You are not authenticated anymore.");
            }
        }
    }
}
