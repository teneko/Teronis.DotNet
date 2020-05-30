

namespace Teronis.Identity.Entities
{
    public interface IConvertUser<UserType, UserCreationType>
        where UserType : IUserEntity
    {
        UserCreationType Convert(UserType source, string[]? roles);
    }
}
