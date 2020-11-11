using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IConvertUserDescriptor<UserDescriptorType, UserType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IAccountUserEntity
    {
        UserType Convert(UserDescriptorType source);
    }
}
