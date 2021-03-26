// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public class UserCreationDatatransject
    {
        internal UserCreationDatatransject() { }

        public string? UserName { get; internal set; }
        public string? Email { get; internal set; }
        public string[]? Roles { get; internal set; }
        public string? PhoneNumber { get; internal set; }
    }
}
