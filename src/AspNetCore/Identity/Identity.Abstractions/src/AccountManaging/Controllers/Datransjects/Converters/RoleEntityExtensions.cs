using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public static class RoleEntityExtensions
    {
        /// <summary>
        /// Creates role view from role entity.
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public static RoleCreationDatatransject ToRoleCreation(this RoleEntity roleEntity)
        {
            return new RoleCreationDatatransject() {
                RoleName = roleEntity.Name
            };
        }
    }
}
