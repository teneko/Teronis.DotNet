using System.Threading.Tasks;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager<UserType, RoleType>
    {
        Task<IServiceResult<RoleType>> CreateRoleAsync(RoleType roleEntity);
        Task<IServiceResult<RoleType>> CreateRoleIfNotExistsAsync(RoleType roleEntity);
        Task<IServiceResult<UserType>> CreateUserAsync(UserType userEntity, string password, string[]? roles = null);
        Task<IServiceResult<UserType>> CreateUserIfNotExistsAsync(UserType userEntity, string password, string[]? roles = null);
    }
}
