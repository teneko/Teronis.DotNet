namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleCreationDatatransject
    {
        public string RoleName { get; internal set; }

        internal RoleCreationDatatransject() =>
            RoleName = null!;
    }
}
