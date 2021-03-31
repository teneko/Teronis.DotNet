// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModule : JSLocalObject, IJSModule
    {
        /// <summary>
        /// Module name or path of the module.
        /// </summary>
        public string ModuleNameOrPath { get; }

        public JSModule(
            IJSObjectReference jsObjectReference,
            string moduleName,
            IJSObjectInterceptor? jsObjectInterceptor)
            : base(jsObjectReference, jsObjectInterceptor) =>
            ModuleNameOrPath = moduleName ?? throw new System.ArgumentNullException(nameof(moduleName));
    }
}
