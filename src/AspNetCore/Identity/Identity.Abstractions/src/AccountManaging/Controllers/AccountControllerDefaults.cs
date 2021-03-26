// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers
{
    public static class AccountControllerDefaults
    {
        private const string policyPrefix = nameof(Teronis)
            + "." + nameof(Identity)
            + "." + nameof(AccountManaging)
            + "." + nameof(Controllers)
            + "." + "AccountController"
            + ".";

        public const string CanCreateUserPolicy = policyPrefix + "CanCreateUser";
        public const string CanCreateRolePolicy = policyPrefix + "CanCreateRole";
    }
}
