

namespace Teronis.Identity.Datransjects
{
    public interface IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        where RoleDescriptorType : IRoleDescriptor
    {
        RoleType Convert(RoleDescriptorType source);
    }
}
