using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public class RoleDescriptorRoleConverter : IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>
    {
        public RoleEntity Convert(RoleDescriptorDatatransject source) =>
            source.ToRole();
    }
}
