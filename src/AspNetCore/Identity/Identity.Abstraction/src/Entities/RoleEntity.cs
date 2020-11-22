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
