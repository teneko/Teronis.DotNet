namespace Teronis.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IUserDescriptor
    {
        string UserName { get; }
        string Password { get; }
        string[]? Roles { get; }
    }
}
