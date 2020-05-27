using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    //[ApiController]
    //[Route("api/users")]
    public class UsersController<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where RoleDescriptorType : IRoleDescriptor
    {
        private readonly IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountManager;

        public UsersController(IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType> accountManager) =>
            this.accountManager = accountManager;

        //[HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        //[ProducesResponseType(typeof(UserCreationDatatransject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IServiceResult<UserCreationType>> Create(UserDescriptorType userCreateModel) =>
            accountManager.CreateUserAsync(userCreateModel);
    }
}
