using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public interface IBearerSignInManager
    {
        /// <summary>
        /// Creates sign-in tokens: 1. Search for user by claim <see cref="ClaimTypes.NameIdentifier"/>
        /// 2. Create tokens and store refresh token
        /// 3. Return tokens
        /// </summary>
        /// <param name="principal">
        /// The principal represents the authenticated user. It must be guranteed that the claim <see cref="ClaimTypes.NameIdentifier"/> exists.
        /// The claim <see cref="ClaimsIdentityOptions.SecurityStampClaimType"/> when existing is a logout mechanism. If you use refresh token after
        /// you changed the security stamp of the user the token is invalid.
        /// </param>
        /// <returns>The sign-in tokens: the refresh and the access token</returns>
        Task<IServiceResult<SignInTokens>> CreateTokensAsync(ClaimsPrincipal principal);
        /// <summary>
        /// Creates sign-in tokens: 1. Search for user by claim <see cref="ClaimTypes.NameIdentifier"/>
        /// 2. Find stored refresh token and delete it
        /// 2. Create tokens and store new refresh token
        /// 3. Return tokens
        /// </summary>
        /// <param name="principal">
        /// The principal represents the authenticated user. It must be guranteed that the claim <see cref="ClaimTypes.NameIdentifier"/> exists.
        /// The claim <see cref="ClaimsIdentityOptions.SecurityStampClaimType"/> when existing is a logout mechanism. If you use refresh token after
        /// you changed the security stamp of the user the token is invalid.
        /// </param>
        /// <returns>The sign-in tokens: the refresh and the access token</returns>
        Task<IServiceResult<SignInTokens>> CreateNextTokensAsync(ClaimsPrincipal principal);
    }
}
