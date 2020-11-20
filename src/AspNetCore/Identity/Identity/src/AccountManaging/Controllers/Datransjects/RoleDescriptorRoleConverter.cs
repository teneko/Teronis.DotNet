using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleDescriptorRoleConverter : IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>
    {
        public RoleEntity Convert(RoleDescriptorDatatransject source) =>
            source.ToRole();
    }
}
