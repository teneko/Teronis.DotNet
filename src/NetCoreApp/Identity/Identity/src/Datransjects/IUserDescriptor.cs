

namespace Teronis.Identity.Datransjects
{
    public interface IUserDescriptor
    {
        string UserName { get; }
        string Password { get; }
        string[]? Roles { get; }
    }
}
