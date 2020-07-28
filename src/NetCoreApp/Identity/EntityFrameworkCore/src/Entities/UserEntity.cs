using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
{
    public class UserEntity : IdentityUser, IAccountUserEntity, IBearerUserEntity
    {
        public UserEntity() : base() { }

        public UserEntity(string userName) 
            : base(userName) 
        { }
    }
}
