using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public class RoleRoleCreationConverter : IConvertRole<RoleEntity, RoleCreationDatatransject>
    {
        public RoleCreationDatatransject Convert(RoleEntity source) =>
            source.ToRoleCreation();
    }
}
