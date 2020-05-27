using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager : IAccountManager<UserDescriptorDatatransject, UserCreationDatatransject, RoleDescriptorDatatransject, RoleCreationDatatransject>
    {}
}
