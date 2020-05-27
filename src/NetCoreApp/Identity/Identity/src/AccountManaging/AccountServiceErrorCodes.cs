using Teronis.ObjectModel.Annotations;

namespace Teronis.Identity.AccountManaging
{
    public enum AccountServiceErrorCodes
    {
        [StringValue("RoleAlreadyCreated")]
        RoleAlreadyCreatedErrorCode,
        [StringValue("UserAlreadyCreated")]
        UserAlreadyCreatedErrorCode
    }
}
