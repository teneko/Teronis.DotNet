using System.Security.Claims;
using Teronis.Identity.Entities;
using Teronis.Identity.Presenters.Generic.ObjectModel;
using Teronis.Identity.Presenters.ObjectModel;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.SignInServicing
{
    public class SignInServiceContext : IServiceResultInjection<SignInTokens>
    {
        public ClaimsPrincipal Principal { get; }
        public IServiceResult<SignInTokens>? Result { get; private set; }
        public UserEntity? User { get; set; }
        public string? AccessToken { get; set; }
        public RefreshTokenEntity? RefreshTokenEntity { get; set; }
        public string? RefreshToken { get; set; }

        public SignInServiceContext(ClaimsPrincipal principal)
            => Principal = principal ?? throw SignInServiceThrowHelper.GetPrincipalNullException(nameof(principal));

        public void SetResult(IServiceResult<SignInTokens> result) =>
            Result = result;

        public IServiceResultDelegatedFactory<SignInTokens> SetResult() =>
            new ServiceResultDelegatedFactory<SignInTokens>(this);
    }
}
