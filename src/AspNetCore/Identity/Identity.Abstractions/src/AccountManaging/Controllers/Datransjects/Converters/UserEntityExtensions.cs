using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public static class UserEntityExtensions
    {
        public static UserCreationDatatransject ToUserCreation(this UserEntity userEntity, string[]? roles = null)
        {
            return new UserCreationDatatransject() {
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber,
                Roles = roles
            };
        }
    }
}
