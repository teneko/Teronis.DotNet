using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleDescriptorDatatransject : IRoleDescriptor
    {
        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; } = null!;
    }
}
