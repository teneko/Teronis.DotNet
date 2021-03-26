// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Reflection.Caching
{
    public delegate void PropertyCachedEvent<TProperty>(object sender, PropertyCachedEventArgs<TProperty> args);
}
