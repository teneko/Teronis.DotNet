// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Authorization;

namespace Teronis.Microsoft.AspNetCore.Authorization.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        /// <summary>
        /// Adds the same policy for each specified name.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="policy"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static AuthorizationOptions AddPolicyForEach(this AuthorizationOptions options, AuthorizationPolicy policy, params string[] names)
        {
            foreach (var name in names) {
                options.AddPolicy(name, policy);
            }

            return options;
        }

        /// <summary>
        /// Adds the same policy for each specified name.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="policyBuilder"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static AuthorizationOptions AddPolicyForEach(this AuthorizationOptions options, Action<AuthorizationPolicyBuilder> policyBuilder, params string[] names)
        {
            foreach (var name in names) {
                options.AddPolicy(name, policyBuilder);
            }

            return options;
        }
    }
}
