// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleDescriptorRoleConverter : IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>
    {
        public RoleEntity Convert(RoleDescriptorDatatransject source) =>
            source.ToRole();
    }
}
