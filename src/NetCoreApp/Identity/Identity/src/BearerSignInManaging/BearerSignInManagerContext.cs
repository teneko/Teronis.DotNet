using System.Security.Claims;
using Teronis.Identity.Entities;
using Teronis.Identity.Presenters.Generic.ObjectModel;
using Teronis.Identity.Presenters.ObjectModel;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.BearerSignInManaging
{
    public class BearerSignInManagerContext<UserEntityType, BearerTokenType> : IServiceResultInjection<SignInTokens>
        where UserEntityType : class, IUserEntity
        where BearerTokenType : class, IBearerTokenEntity
    {
        public ClaimsPrincipal Principal { get; }
        public IServiceResult<SignInTokens>? Result { get; private set; }
        public UserEntityType? User { get; set; }
        public string? AccessToken { get; set; }
        public BearerTokenType? RefreshTokenEntity { get; set; }
        public string? RefreshToken { get; set; }

        public BearerSignInManagerContext(ClaimsPrincipal principal)
            => Principal = principal ?? throw BearerSignInManagerThrowHelper.GetPrincipalNullException(nameof(principal));

        public void SetResult(IServiceResult<SignInTokens> result) =>
            Result = result;

        public IServiceResultDelegatedFactory<SignInTokens> SetResult() =>
            new ServiceResultDelegatedFactory<SignInTokens>(this);
    }
}
