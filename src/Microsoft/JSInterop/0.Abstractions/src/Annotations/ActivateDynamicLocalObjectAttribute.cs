// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// Decoratable on methods that return ValueTask{IJSLocalObject} in proxy interfaces
    /// where IJSLocalObject can be any interface that derives from it. Used in interceptor.
    /// </summary>
    public class ActivateDynamicLocalObjectAttribute : ActivateLocalObjectAttribute
    {
        public Type? InterfaceToBeProxied { get; set; }
    }
}
