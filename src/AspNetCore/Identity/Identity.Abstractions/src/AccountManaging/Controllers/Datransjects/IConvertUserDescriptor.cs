// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IConvertUserDescriptor<UserDescriptorType, UserType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IAccountUserEntity
    {
        UserType Convert(UserDescriptorType source);
    }
}
