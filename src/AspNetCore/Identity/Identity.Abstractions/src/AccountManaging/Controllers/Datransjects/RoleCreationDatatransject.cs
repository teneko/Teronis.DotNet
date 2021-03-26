// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleCreationDatatransject
    {
        public string RoleName { get; internal set; }

        internal RoleCreationDatatransject() =>
            RoleName = null!;
    }
}
