// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module
{
    public interface IJSModuleActivator
    {
        ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath, JSModuleCreationOptions? creationOptions = null);
    }
}
