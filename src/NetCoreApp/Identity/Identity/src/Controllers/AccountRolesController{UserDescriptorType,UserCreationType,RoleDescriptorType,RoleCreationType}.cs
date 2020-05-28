using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    public abstract class AccountRolesController<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where RoleDescriptorType : IRoleDescriptor
    {
        private readonly IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountService;

        public AccountRolesController(IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountService) =>
            this.accountService = accountService;

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual Task<IServiceResult<RoleCreationType>> Create(RoleDescriptorType roleDescriptor) =>
            accountService.CreateRoleAsync(roleDescriptor);
    }
}
