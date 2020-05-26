using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.AccountServicing;
using Teronis.Identity.AccountServicing.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController
    {
        private readonly AccountService accountService;

        public UsersController(AccountService accountService) =>
             this.accountService = accountService;

        [HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(UserCreationDatatransject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IServiceResult<UserCreationDatatransject>> Create(UserDescriptorDatatransject userCreateModel) =>
            accountService.CreateUserAsync(userCreateModel);
    }
}
