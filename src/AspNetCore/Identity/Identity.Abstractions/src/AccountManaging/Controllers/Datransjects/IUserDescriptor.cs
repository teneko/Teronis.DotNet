// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects
{
    public interface IUserDescriptor
    {
        string UserName { get; }
        string Password { get; }
        string[]? Roles { get; }
    }
}
