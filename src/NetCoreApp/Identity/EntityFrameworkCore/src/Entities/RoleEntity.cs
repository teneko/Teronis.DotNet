using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
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
