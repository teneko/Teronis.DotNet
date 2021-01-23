

using Teronis.AspNetCore.Identity.Bearer.SignInManaging;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public static class BearerSignInManagerDefaults
    {
        public const string BearerSignInManagerRefreshTokenIdClaimType = nameof(BearerSignInManager) + "." +  nameof(SignInTokens.RefreshToken) + "Id";
    }
}
