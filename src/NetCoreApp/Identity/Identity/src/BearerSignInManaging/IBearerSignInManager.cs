using System.Security.Claims;
using System.Threading.Tasks;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public interface IBearerSignInManager
    {
        Task<IServiceResult<SignInTokens>> CreateInitialSignInTokensAsync(ClaimsPrincipal principal);
        Task<IServiceResult<SignInTokens>> CreateNextSignInTokensAsync(ClaimsPrincipal principal);
    }
}