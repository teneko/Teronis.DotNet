using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    //[ApiController]
    //[Route("api/roles")]
    public class RolesController<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where RoleDescriptorType : IRoleDescriptor
    {
        private readonly IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountService;

        public RolesController(IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountService) =>
            this.accountService = accountService;

        //[HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        //[ProducesResponseType(typeof(UserType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IServiceResult<RoleCreationType>> Create(RoleDescriptorType roleDescriptor) =>
            accountService.CreateRoleAsync(roleDescriptor);
    }
}
