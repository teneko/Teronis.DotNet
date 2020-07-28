using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.Bearer.Authentication;
using Teronis.Identity.Bearer;

namespace Teronis.Identity.Controllers
{
    [Route("api/sign-in")]
    public class BearerSignInController : Controller
    {
        private readonly IBearerSignInManager signInManager;

        public BearerSignInController(IBearerSignInManager signInManager) =>
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
