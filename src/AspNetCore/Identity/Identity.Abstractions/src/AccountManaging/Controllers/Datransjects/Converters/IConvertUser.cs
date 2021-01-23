using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public interface IConvertUser<UserType, UserCreationType>
        where UserType : IAccountUserEntity
    {
        UserCreationType Convert(UserType source, string[]? roles);
    }
}
