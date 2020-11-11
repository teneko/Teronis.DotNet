namespace Teronis.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public interface IConvertRole<RoleType, RoleCreationType>
    {
        RoleCreationType Convert(RoleType source);
    }
}
