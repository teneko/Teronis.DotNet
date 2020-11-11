using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.AccountManaging.Controllers.Datransjects
{
    public class UserDescriptorDatatransject : IUserDescriptor
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; } = null!;
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; } = null!;
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; } = null!;
        public string[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
