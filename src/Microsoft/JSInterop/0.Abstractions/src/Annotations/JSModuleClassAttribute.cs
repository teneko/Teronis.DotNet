// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    /// <summary>
    /// Decoratable on class. It provides
    /// <see cref="JSModuleAttributeBase.ModuleNameOrPath"/>
    /// to those properties with facade attribute that are using
    /// parameterless constructor but not
    /// <see cref="AssignModuleAttribute(string)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class JSModuleClassAttribute : JSModuleAttributeBase
    {
        public JSModuleClassAttribute(string ModuleNameOrPath)
            : base(ModuleNameOrPath) { }
    }
}
