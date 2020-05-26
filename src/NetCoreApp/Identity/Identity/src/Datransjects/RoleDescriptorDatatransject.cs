using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teronis.Identity.Datransjects
{
    public class RoleDescriptorDatatransject
    {
        [Required(AllowEmptyStrings = true)]
        public string Role { get; set; } = null!;
    }
}
