using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
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
