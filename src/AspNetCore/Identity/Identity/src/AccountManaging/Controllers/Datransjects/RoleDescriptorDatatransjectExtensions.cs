using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public static class RoleDescriptorDatatransjectExtensions
    {
        public static RoleEntity ToRole(this RoleDescriptorDatatransject roleDescriptor) =>
            new RoleEntity(roleDescriptor.RoleName);
    }
}
