namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public interface IConvertRole<RoleType, RoleCreationType>
    {
        RoleCreationType Convert(RoleType source);
    }
}
