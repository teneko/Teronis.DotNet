// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// Decoratable on methods that return ValueTask{IJSLocalObject} in proxy interfaces. Used in interceptor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ActivateLocalObjectAttribute : LocalObjectAttributeBase
    { }
}
