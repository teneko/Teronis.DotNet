using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public static class UserDescriptorDatatransjectExtensions
    {
        public static UserEntity ToUser(this UserDescriptorDatatransject userDescriptor)
        {
            return new UserEntity(userDescriptor.UserName) {
                Email = userDescriptor.Email,
                PhoneNumber = userDescriptor.PhoneNumber
            };
        }
    }
}
