// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner
{
    public interface IValueAssignerContext
    {
        YetNullable<IAsyncDisposable> ValueResult { get; set; }
    }
}