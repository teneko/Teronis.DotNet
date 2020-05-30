using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public class UserDescriptorUserConverter : IConvertUserDescriptor<UserDescriptorDatatransject, UserEntity>
    {
        public UserEntity Convert(UserDescriptorDatatransject source) =>
            source.ToUser();
    }
}
