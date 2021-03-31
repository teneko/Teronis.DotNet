// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public static class IJSDynamicLocalObjectActivatorExtensions
    {
        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, string objectName)
            where T : class, IJSLocalObject =>
            (T)await jsDynamicLocalObjectActivator.CreateInstanceAsync(typeof(T), objectName);

        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, IJSObjectReference jsObjectReference, string objectName)
            where T : class, IJSLocalObject =>
            (T)await jsDynamicLocalObjectActivator.CreateInstanceAsync(typeof(T), jsObjectReference, objectName);

        public static ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator, IJSLocalObject jsLocalObject, string objectName)
            where T : class, IJSLocalObject =>
            jsDynamicLocalObjectActivator.CreateInstanceAsync<T>(jsLocalObject.JSObjectReference, objectName);
    }
}
