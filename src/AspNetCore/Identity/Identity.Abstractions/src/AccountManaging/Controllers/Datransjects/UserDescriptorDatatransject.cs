// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
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
