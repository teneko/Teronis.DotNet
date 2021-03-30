// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Interceptors;

namespace Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception
{
    public interface INestedOwningDynamicObject
    {
        [JSDynamicProxyActivatingInterceptor]
        ValueTask<INestedDynamicObject> GetNestedObjectAsync();
    }
}
