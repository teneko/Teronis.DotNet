using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    /// <summary>
    /// Provides user entity conversion.
    /// </summary>
    /// <typeparam name="UserType"></typeparam>
    /// <typeparam name="UserCreationType"></typeparam>
    public interface IConvertUser<UserType, UserCreationType>
        where UserType : IAccountUserEntity
    {
        /// <summary>
        /// Converts user entity to user view.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        UserCreationType Convert(UserType source, string[]? roles);
    }
}
