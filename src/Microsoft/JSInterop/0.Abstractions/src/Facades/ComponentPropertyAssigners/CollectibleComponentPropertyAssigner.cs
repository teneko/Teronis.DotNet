// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners
{
    /// <summary>
    /// Used in post configuration. Intendeted to be registered 
    /// as <see cref="ICollectibleComponentPropertyAssigner"/>.
    /// in dependency injection.
    /// </summary>
    internal sealed class CollectibleComponentPropertyAssigner : ICollectibleComponentPropertyAssigner
    {
        public Type ImplementationType { get; }

        public CollectibleComponentPropertyAssigner(Type implementationType) =>
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
    }
}
