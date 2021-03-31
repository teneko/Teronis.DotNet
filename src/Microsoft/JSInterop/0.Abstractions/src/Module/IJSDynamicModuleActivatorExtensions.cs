// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module
{
    public static class IJSDynamicModuleActivatorExtensions
    {
        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicModuleActivator jsDynamicModuleActivator, string moduleNameOrPath)
            where T : class, IJSModule =>
            (T)await jsDynamicModuleActivator.CreateInstanceAsync(typeof(T), moduleNameOrPath);
    }
}
