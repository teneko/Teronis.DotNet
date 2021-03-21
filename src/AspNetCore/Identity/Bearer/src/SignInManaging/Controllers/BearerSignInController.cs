using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.AspNetCore.Identity.Bearer.Authentication;

namespace Teronis.AspNetCore.Identity.Bearer.SignInManaging.Controllers
{
    [ApiController]
    public class BearerSignInController<TNotUseful> : Controller
        where TNotUseful : ISingleton
    {
        private readonly IBearerSignInManager signInManager;

        public BearerSignInController(IBearerSignInManager signInManager) =>
            this.signInManager = signInManager;

        [HttpGet("authenticate")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // Very important, otherwise the user entity cannot be resolved.
        [Authorize(AuthenticationSchemes = AuthenticationDefaults.IdentityBasicScheme)]
        public async Task<IActionResult> AuthenticateAsync() =>
            Json(await signInManager.CreateTokensAsync(HttpContext.User));

        [HttpGet("refresh-token")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(AuthenticationSchemes = AuthenticationDefaults.IdentityRefreshTokenBearerScheme)]
        public async Task<IActionResult> RefreshTokenAsync() =>
            Json(await signInManager.CreateNextTokensAsync(HttpContext.User));
    }
}
