// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public interface IJSDynamicModulePropertyAssigner
    {
        ValueTask<YetNullable<IAsyncDisposable>> TryAssignProperty(IDefinition componentProperty);
    }
}
