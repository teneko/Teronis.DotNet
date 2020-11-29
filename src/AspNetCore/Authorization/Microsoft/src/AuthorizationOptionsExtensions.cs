using System;

namespace Microsoft.AspNetCore.Authorization.Teronis
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddPolicy(this AuthorizationOptions options, AuthorizationPolicy policy, params string[] names)
        {
            foreach (var name in names) {
                options.AddPolicy(name, policy);
            }

            return options;
        }

        public static AuthorizationOptions AddPolicy(this AuthorizationOptions options, Action<AuthorizationPolicyBuilder> policyBuilder, params string[] names)
        {
            foreach (var name in names) {
                options.AddPolicy(name, policyBuilder);
            }

            return options;
        }
    }
}
