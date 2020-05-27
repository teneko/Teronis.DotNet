using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.Authentication;
using Teronis.Identity.BearerSignInManaging;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace Teronis.Identity.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly BearerSignInManager signInService;

        public UserController(BearerSignInManager signInService) =>
            this.signInService = signInService;

        [HttpGet("authenticate")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // Very important, otherwise the user entity cannot be resolved.
        [Authorize(AuthenticationSchemes = SignInServiceAuthenticationDefaults.RefreshTokenBasicScheme)]
        public async Task<IActionResult> Authenticate() =>
            await signInService.CreateInitialSignInTokensAsync(HttpContext.User);

        [HttpGet("refreshToken")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(SignInTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = SignInServiceAuthenticationDefaults.RefreshTokenBearerScheme)]
        public async Task<IActionResult> RefreshToken() =>
            await signInService.CreateNextSignInTokensAsync(HttpContext.User);
    }
}
