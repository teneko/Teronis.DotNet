using Teronis.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public static class RoleEntityExtensions
    {
        public static RoleCreationDatatransject ToRoleCreation(this RoleEntity roleEntity)
        {
            return new RoleCreationDatatransject() {
                RoleName = roleEntity.Name
            };
        }
    }
}
