using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.Datransjects
{
    public class RoleDescriptorDatatransject : IRoleDescriptor
    {
        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; } = null!;
    }
}
