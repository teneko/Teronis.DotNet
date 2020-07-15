using System;
using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
{
    public class UserEntity : IdentityUser, IAccountUserEntity, IUserEntity
    {
        public UserEntity() : base() { }

        public UserEntity(string userName) : base(userName ?? throw new ArgumentNullException(userName)) { }
    }
}
