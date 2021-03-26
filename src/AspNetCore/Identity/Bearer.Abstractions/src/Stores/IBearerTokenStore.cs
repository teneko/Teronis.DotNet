// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
{
    public interface IBearerTokenStore : IBearerTokenStore<BearerTokenEntity>
    { }
}
