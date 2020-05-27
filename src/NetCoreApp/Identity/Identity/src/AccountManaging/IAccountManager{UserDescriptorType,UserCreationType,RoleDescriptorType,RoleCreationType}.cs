using System.Threading.Tasks;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where RoleDescriptorType : IRoleDescriptor
    {
        Task<IServiceResult<RoleCreationType>> CreateRoleAsync(RoleDescriptorType roleDescriptor);
        Task<IServiceResult<UserCreationType>> CreateUserAsync(UserDescriptorType userDescriptor);
    }
}
