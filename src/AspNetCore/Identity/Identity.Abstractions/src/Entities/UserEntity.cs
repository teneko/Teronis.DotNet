// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Identity;

namespace Teronis.AspNetCore.Identity.Entities
{
    public class UserEntity : IdentityUser, IAccountUserEntity, IBearerUserEntity
    {
        public UserEntity() : base() { }

        public UserEntity(string userName) 
            : base(userName) 
        { }
    }
}
