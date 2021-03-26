// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public static class RoleDescriptorDatatransjectExtensions
    {
        public static RoleEntity ToRole(this RoleDescriptorDatatransject roleDescriptor) =>
            new RoleEntity(roleDescriptor.RoleName);
    }
}
