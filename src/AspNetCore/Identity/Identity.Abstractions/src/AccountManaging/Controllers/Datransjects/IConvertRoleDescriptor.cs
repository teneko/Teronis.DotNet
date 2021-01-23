namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        where RoleDescriptorType : IRoleDescriptor
    {
        RoleType Convert(RoleDescriptorType source);
    }
}
