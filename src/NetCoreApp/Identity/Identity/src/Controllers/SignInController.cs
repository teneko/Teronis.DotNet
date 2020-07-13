using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Teronis.Identity.Authentication;
using Teronis.Identity.BearerSignInManaging;

namespace Teronis.Identity.Controllers
{
    [Route("api/sign-in")]
    public class SignInController : Controller
    {
        private readonly IBearerSignInManager signInManager;

        public SignInController(IBearerSignInManager signInManager) =>
            this.signInManager = signInManager;

        [HttpGet("authenticate")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // Very important, otherwise the user entity cannot be resolved.
        [Authorize(AuthenticationSchemes = AuthenticationDefaults.IdentityBasicScheme)]
        public async Task<IActionResult> AuthenticateAsync() =>
            await signInManager.CreateTokensAsync(HttpContext.User);

        [HttpGet("refreshToken")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = AuthenticationDefaults.IdentityRefreshTokenBearerScheme)]
        public async Task<IActionResult> RefreshTokenAsync() =>
            await signInManager.CreateNextTokensAsync(HttpContext.User);
    }
}
