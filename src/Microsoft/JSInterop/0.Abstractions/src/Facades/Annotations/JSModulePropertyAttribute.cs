// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JSModulePropertyAttribute : JSModuleAttributeBase
    {
        /// <inheritdoc/>
        public JSModulePropertyAttribute()
        { }

        /// <inheritdoc/>
        public JSModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }
    }
}
