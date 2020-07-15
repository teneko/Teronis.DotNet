using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public interface IConvertUserDescriptor<UserDescriptorType, UserType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IAccountUserEntity
    {
        UserType Convert(UserDescriptorType source);
    }
}
