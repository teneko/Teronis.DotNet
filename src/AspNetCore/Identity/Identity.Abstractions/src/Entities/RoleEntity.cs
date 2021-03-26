// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Identity;

namespace Teronis.AspNetCore.Identity.Entities
{
    public class RoleEntity : IdentityRole, IAccountRoleEntity
    {
        string IAccountRoleEntity.RoleName => Name;

        public RoleEntity()
            : base() { }

        public RoleEntity(string roleName)
            : base(roleName) { }
    }
}
