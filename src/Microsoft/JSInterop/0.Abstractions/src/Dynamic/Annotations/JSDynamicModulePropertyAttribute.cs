// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Annotations
{
    public class JSDynamicModulePropertyAttribute : JSModulePropertyAttribute
    {
        public JSDynamicModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }

        public JSDynamicModulePropertyAttribute()
        { }
    }
}
