

namespace Teronis.Identity.AccountManaging.Datatransjects
{
    public class RoleCreationDatatransject
    {
        public string Role { get; internal set; }

        internal RoleCreationDatatransject() =>
            Role = null!;
    }
}
