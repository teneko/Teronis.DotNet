// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public class JSDynamicModulePropertyAttribute : JSModulePropertyAttribute
    {
        public JSDynamicModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }

        public JSDynamicModulePropertyAttribute()
        { }
    }
}
