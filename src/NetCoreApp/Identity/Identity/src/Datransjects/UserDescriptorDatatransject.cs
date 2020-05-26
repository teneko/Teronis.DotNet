using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.Datransjects
{
    public class UserDescriptorDatatransject
    {
        [Required(AllowEmptyStrings = true)]
        public string UserName { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Email { get; set; } = null!;
        public string[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
