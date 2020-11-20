namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class UserCreationDatatransject
    {
        internal UserCreationDatatransject() { }

        public string? UserName { get; internal set; }
        public string? Email { get; internal set; }
        public string[]? Roles { get; internal set; }
        public string? PhoneNumber { get; internal set; }
    }
}
