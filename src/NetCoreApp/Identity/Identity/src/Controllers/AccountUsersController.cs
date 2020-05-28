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
    [Route("api/account/users")]
    public class AccountUsersController
    {
        private readonly IAccountManager accountManager;

        public AccountUsersController(IAccountManager accountManager) =>
            this.accountManager = accountManager;

        [HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(RoleCreationDatatransject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IServiceResult<UserCreationDatatransject>> Create(UserDescriptorDatatransject userDescriptor)
        {
            var userEntity = new UserEntity(userDescriptor.UserName) {
                Email = userDescriptor.Email,
                PhoneNumber = userDescriptor.PhoneNumber
            };

            var createUserResult = await accountManager.CreateUserAsync(userEntity, userDescriptor.Password, roles: userDescriptor.Roles);

            if (createUserResult.Succeeded) {
                var createdUser = createUserResult.Content().ToUserCreation(roles: userDescriptor.Roles);
                return createUserResult.CopyButSucceededWithContent(createdUser);
            }

            return createUserResult.CopyButFailed<UserEntity, UserCreationDatatransject>();
        }
    }
}
