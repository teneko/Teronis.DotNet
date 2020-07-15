

namespace Teronis.Identity.Entities
{
    public interface IConvertUser<UserType, UserCreationType>
        where UserType : IAccountUserEntity
    {
        UserCreationType Convert(UserType source, string[]? roles);
    }
}
