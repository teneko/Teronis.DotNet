// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception
{
    public interface INestedDynamicObject : IAsyncDisposable
    {
        ValueTask<string> ReturnsThisNameAsync();
    }
}
