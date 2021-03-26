// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        where RoleDescriptorType : IRoleDescriptor
    {
        RoleType Convert(RoleDescriptorType source);
    }
}
