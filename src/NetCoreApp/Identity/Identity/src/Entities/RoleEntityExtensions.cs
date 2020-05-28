using Teronis.Identity.AccountManaging.Datatransjects;

namespace Teronis.Identity.Entities
{
    public static class RoleEntityExtensions
    {
        public static RoleCreationDatatransject ToRoleCreation(this RoleEntity roleEntity)
        {
            return new RoleCreationDatatransject() {
                Role = roleEntity.Id
            };
        }
    }
}
