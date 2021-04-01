// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AssignModuleAttribute : JSModuleAttributeBase
    {
        /// <inheritdoc/>
        public AssignModuleAttribute()
        { }

        /// <inheritdoc/>
        public AssignModuleAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }
    }
}
