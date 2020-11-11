using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public interface IConvertUser<UserType, UserCreationType>
        where UserType : IAccountUserEntity
    {
        UserCreationType Convert(UserType source, string[]? roles);
    }
}
