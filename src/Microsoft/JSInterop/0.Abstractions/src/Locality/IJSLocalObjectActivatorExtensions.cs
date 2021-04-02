// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public static class IJSLocalObjectActivatorExtensions
    {
        public static ValueTask<IJSLocalObject> CreateInstanceAsync(this IJSLocalObjectActivator jsLocalObjectActivator, IJSLocalObject jsLocalObject, string objectName) =>
            jsLocalObjectActivator.CreateInstanceAsync(jsLocalObject.ObjectReference, objectName);
    }
}
