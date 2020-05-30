using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public static class RoleDescriptorDatatransjectExtensions
    {
        public static RoleEntity ToRole(this RoleDescriptorDatatransject roleDescriptor) =>
            new RoleEntity(roleDescriptor.RoleName);
    }
}
