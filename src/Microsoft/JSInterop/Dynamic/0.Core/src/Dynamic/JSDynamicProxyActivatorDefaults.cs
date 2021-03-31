// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class JSDynamicProxyActivatorDefaults
    {
        public const BindingFlags PROXY_INTERFACE__METHOD_BINDING_FLAGS = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;
    }
}
