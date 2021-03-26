// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop
{
    public delegate void InstanceActivatedDelegate<in T>(T activatedInstance)
        where T : IAsyncDisposable;
}
