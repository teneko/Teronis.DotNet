using System;

namespace Teronis.Identity.SignInServicing
{
    public static class SignInServiceThrowHelper
    {
        public static ArgumentNullException GetPrincipalNullException(string paramName)
            => new ArgumentNullException(paramName);

        public static ArgumentNullException GetContextArgumentException(string paramName)
            => new ArgumentNullException($"{nameof(SignInServiceContext)}.{paramName}");
    }
}
