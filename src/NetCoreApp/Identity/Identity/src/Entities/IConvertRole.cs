

namespace Teronis.Identity.Entities
{
    public interface IConvertRole<RoleType, RoleCreationType>
    {
        RoleCreationType Convert(RoleType source);
    }
}
