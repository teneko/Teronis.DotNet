using System.Security.Claims;
using Teronis.Identity.Entities;
using Teronis.Mvc.ServiceResulting.Generic;
using Teronis.Mvc.ServiceResulting.Generic.ObjectModel;

namespace Teronis.Identity.BearerSignInManaging
{
    public class BearerSignInManagerContext<UserEntityType, BearerTokenType> : IServiceResultInjection<object>
        where UserEntityType : class, IBearerUserEntity
        where BearerTokenType : class, IBearerTokenEntity
    {
        public ClaimsPrincipal Principal { get; }
        public IServiceResult<object>? Result { get; private set; }
        public UserEntityType? User { get; set; }
        public string? AccessToken { get; set; }
        public BearerTokenType? RefreshTokenEntity { get; set; }
        public string? RefreshToken { get; set; }

        public BearerSignInManagerContext(ClaimsPrincipal principal)
            => Principal = principal ?? throw BearerSignInManagerThrowHelper.GetPrincipalNullException(nameof(principal));

        public void SetResult(IServiceResult<object> result) =>
            Result = result;

        public IServiceResultDelegatedFactory<SignInTokens> SetResult() =>
            new ServiceResultDelegatedFactory<SignInTokens>(this);
    }
}
