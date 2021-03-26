// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic.Module
{
    public interface IJSDynamicModuleActivator : IInstanceActivator<IJSModule>
    {
        ValueTask<IJSModule> CreateInstanceAsync(Type interfaceToBeProxied, string moduleNameOrPath);
    }
}
