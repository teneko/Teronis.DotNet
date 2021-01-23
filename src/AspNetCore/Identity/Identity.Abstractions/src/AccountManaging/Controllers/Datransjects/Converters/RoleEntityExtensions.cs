using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
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
