using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.Datransjects
{
    public class RoleDescriptorDatatransject : IRoleDescriptor
    {
        [Required(AllowEmptyStrings = true)]
        public string Role { get; set; } = null!;
    }
}
