// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// This ttribute provides the module name or path
    /// when it has not been specified in 
    /// <see cref="AssignModuleAttribute"/> or
    /// <see cref="AssignDynamicModuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public class JSModuleAttribute : Attribute
    {
        public string NameOrPath { get; }

        public JSModuleAttribute(string nameOrPath) =>
            NameOrPath = nameOrPath;
    }
}
