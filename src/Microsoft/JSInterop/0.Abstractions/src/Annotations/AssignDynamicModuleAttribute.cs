// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public class AssignDynamicModuleAttribute : AssignModuleAttribute
    {
        /// <summary>
        /// Only needed to be specified when you are using
        /// <see cref="AssignCustomFacadeAttribute"/> too.
        /// </summary>
        public Type? InterfaceToBeProxied { get; set; }
    }
}
