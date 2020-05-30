using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Datransjects
{
    public class RoleRoleCreationConverter : IConvertRole<RoleEntity, RoleCreationDatatransject>
    {
        public RoleCreationDatatransject Convert(RoleEntity source) =>
            source.ToRoleCreation();
    }
}
