// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class RoleDescriptorDatatransject : IRoleDescriptor
    {
        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; } = null!;
    }
}
