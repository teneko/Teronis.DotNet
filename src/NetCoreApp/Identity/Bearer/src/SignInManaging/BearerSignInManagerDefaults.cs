

using Teronis.Identity.Bearer.SignInManaging;

namespace Teronis.Identity.Bearer
{
    public static class BearerSignInManagerDefaults
    {
        public const string BearerSignInManagerRefreshTokenIdClaimType = nameof(BearerSignInManager) + "." +  nameof(SignInTokens.RefreshToken) + "Id";
    }
}
