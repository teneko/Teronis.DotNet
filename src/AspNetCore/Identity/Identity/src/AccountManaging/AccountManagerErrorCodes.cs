using Teronis.ObjectModel.Annotations;

namespace Teronis.AspNetCore.Identity.AccountManaging
{
    public enum AccountManagerErrorCodes
    {
        [StringValue("RoleAlreadyCreated")]
        RoleAlreadyCreated,
        [StringValue("UserAlreadyCreated")]
        UserAlreadyCreated
    }
}
