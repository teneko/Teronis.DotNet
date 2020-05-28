using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    [ApiController]
    [Route("api/account/roles")]
    public class AccountRolesController : AccountRolesController<UserDescriptorDatatransject, UserCreationDatatransject, RoleDescriptorDatatransject, RoleCreationDatatransject>
    {
        public AccountRolesController(IAccountManager accountManager)
            : base(accountManager) { }

        [HttpPost("create")]
        [ProducesResponseType(typeof(UserCreationDatatransject), StatusCodes.Status200OK)]
        public override Task<IServiceResult<RoleCreationDatatransject>> Create(RoleDescriptorDatatransject roleDescriptor) =>
            base.Create(roleDescriptor);
    }
}
