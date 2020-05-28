using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager : IAccountManager<UserEntity, RoleEntity>
    { }
}
