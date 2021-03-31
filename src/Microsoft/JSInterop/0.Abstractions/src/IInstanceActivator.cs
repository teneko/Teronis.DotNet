// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop
{
    public interface IInstanceActivator<out T>
        where T : IAsyncDisposable
    {
        event InstanceActivatedDelegate<IAsyncDisposable> AnyInstanceActivated;
        event InstanceActivatedDelegate<T>? InstanceActivated;
    }
}
