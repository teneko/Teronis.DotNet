// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Identity.Entities
{
    /// <summary>
    /// Represents the bearer user entity.
    /// </summary>
    public interface IBearerUserEntity
    {
        string Id { get; }
        string UserName { get; }
        string SecurityStamp { get; }
    }
}
