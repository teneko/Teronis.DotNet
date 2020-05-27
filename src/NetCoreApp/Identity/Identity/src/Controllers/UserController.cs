using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.Authentication;
using Teronis.Identity.BearerSignInManaging;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Controllers
{
    //[ApiController]
    //[Route("api/user")]
    public class UserController : Controller
    {
        private readonly IBearerSignInManager signInManager;

        public UserController(IBearerSignInManager signInManager) =>
            this.signInManager = signInManager;

        //[HttpGet("authenticate")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // Very important, otherwise the user entity cannot be resolved.
        [Authorize(AuthenticationSchemes = SignInServiceAuthenticationDefaults.RefreshTokenBasicScheme)]
        public async Task<IActionResult> Authenticate() =>
            await signInManager.CreateInitialSignInTokensAsync(HttpContext.User);

        //[HttpGet("refreshToken")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = SignInServiceAuthenticationDefaults.RefreshTokenBearerScheme)]
        public async Task<IActionResult> RefreshToken() =>
            await signInManager.CreateNextSignInTokensAsync(HttpContext.User);
    }
}
