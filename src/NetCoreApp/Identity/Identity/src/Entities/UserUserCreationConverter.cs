using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public class UserUserCreationConverter : IConvertUser<UserEntity, UserCreationDatatransject>
    {
        public UserCreationDatatransject Convert(UserEntity source, string[]? roles) =>
            source.ToUserCreation(roles);
    }
}
