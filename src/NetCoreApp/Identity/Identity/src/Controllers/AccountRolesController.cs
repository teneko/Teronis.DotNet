using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    [ApiController]
    [Route("api/account/roles")]
    public class AccountRolesController
    {
        private readonly IAccountManager accountManager;

        public AccountRolesController(IAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(UserCreationDatatransject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IServiceResult<RoleCreationDatatransject>> Create(RoleDescriptorDatatransject roleDescriptor)
        {
            var roleEntity = new RoleEntity(roleDescriptor.Role);
            var createRoleResult = await accountManager.CreateRoleAsync(roleEntity);

            if (createRoleResult.Succeeded) {
                var createdRole = createRoleResult.Content().ToRoleCreation();
                return createRoleResult.CopyButSucceededWithContent(createdRole);
            }

            return createRoleResult.CopyButFailed<RoleEntity, RoleCreationDatatransject>();
        }
    }
}
