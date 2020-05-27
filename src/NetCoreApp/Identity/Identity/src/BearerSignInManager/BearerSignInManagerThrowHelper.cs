using System;

namespace Teronis.Identity.BearerSignInManaging
{
    public static class BearerSignInManagerThrowHelper
    {
        public static ArgumentNullException GetPrincipalNullException(string paramName)
            => new ArgumentNullException(paramName);

        public static ArgumentNullException GetContextArgumentException(string paramName)
            => new ArgumentNullException($"{nameof(BearerSignInManagerContext)}.{paramName}");
    }
}
