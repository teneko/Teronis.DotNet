using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    public class AccountUsersController<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where RoleDescriptorType : IRoleDescriptor
    {
        private readonly IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountManager;

        public AccountUsersController(IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountManager) =>
            this.accountManager = accountManager;

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual Task<IServiceResult<UserCreationType>> Create(UserDescriptorType userCreateModel) =>
            accountManager.CreateUserAsync(userCreateModel);
    }
}
