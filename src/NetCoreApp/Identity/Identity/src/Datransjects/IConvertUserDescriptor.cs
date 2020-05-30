using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public interface IConvertUserDescriptor<UserDescriptorType, UserType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IUserEntity
    {
        UserType Convert(UserDescriptorType source);
    }
}
