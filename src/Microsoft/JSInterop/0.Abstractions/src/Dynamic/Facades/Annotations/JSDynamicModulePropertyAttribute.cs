// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Facades.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.Annotations
{
    public class JSDynamicModulePropertyAttribute : JSModulePropertyAttribute
    {
        public JSDynamicModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }

        public JSDynamicModulePropertyAttribute()
        { }
    }
}
