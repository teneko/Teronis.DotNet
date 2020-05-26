using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
{
    public class RoleEntity : IdentityRole
    {
        public RoleEntity()
            : base() { }

        public RoleEntity(string roleName)
            : base(roleName) { }
    }
}
