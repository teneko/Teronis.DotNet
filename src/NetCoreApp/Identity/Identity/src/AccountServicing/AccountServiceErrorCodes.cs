using Teronis.ObjectModel.Annotations;

namespace Teronis.Identity.AccountServicing
{
    public enum AccountServiceErrorCodes
    {
        [StringValue("RoleAlreadyCreated")]
        RoleAlreadyCreatedErrorCode,
        [StringValue("UserAlreadyCreated")]
        UserAlreadyCreatedErrorCode
    }
}
