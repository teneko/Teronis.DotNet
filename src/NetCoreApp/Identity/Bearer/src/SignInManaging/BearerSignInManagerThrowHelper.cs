using System;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Bearer
{
    public static class BearerSignInManagerThrowHelper
    {
        public static ArgumentNullException GetPrincipalNullException(string paramName)
            => new ArgumentNullException(paramName);

        public static ArgumentNullException GetContextArgumentException(string paramName)
            => new ArgumentNullException($"{nameof(BearerSignInManagerContext<UserEntity, BearerTokenEntity>)}.{paramName}");
    }
}
