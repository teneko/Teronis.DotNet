using Teronis.ObjectModel.Annotations;

namespace Teronis.Identity.AccountManaging
{
    public enum AccountManagerErrorCodes
    {
        [StringValue("RoleAlreadyCreated")]
        RoleAlreadyCreated,
        [StringValue("UserAlreadyCreated")]
        UserAlreadyCreated
    }
}
