using System;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners
{
    /// <summary>
    /// Used in post configuration. Intendeted to be registered 
    /// as <see cref="ICollectibleComponentPropertyAssigner"/>.
    /// in dependency injection.
    /// </summary>
    internal interface ICollectibleComponentPropertyAssigner
    {
        Type ImplementationType { get; }
    }
}
