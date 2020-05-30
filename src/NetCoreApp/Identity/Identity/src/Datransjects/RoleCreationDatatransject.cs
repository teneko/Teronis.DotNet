

namespace Teronis.Identity.AccountManaging.Datatransjects
{
    public class RoleCreationDatatransject
    {
        public string RoleName { get; internal set; }

        internal RoleCreationDatatransject() =>
            RoleName = null!;
    }
}
