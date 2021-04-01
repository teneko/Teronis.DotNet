// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public class ReturnDynamicModuleAttribute : AssignModuleAttribute
    {
        public ReturnDynamicModuleAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }

        public ReturnDynamicModuleAttribute()
        { }
    }
}
