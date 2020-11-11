using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleDescriptorRoleConverter : IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>
    {
        public RoleEntity Convert(RoleDescriptorDatatransject source) =>
            source.ToRole();
    }
}
