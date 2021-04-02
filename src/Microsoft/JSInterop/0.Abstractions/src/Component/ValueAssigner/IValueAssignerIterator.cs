// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Component.ValueAssigner.ValueAssignerContext;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner
{
    public interface IValueAssignerIteratorExecutor : ITreeIteratorExecutor<ValueAssignerEntry>
    {
        IReadOnlyList<IValueAssigner> ProvideValueAssigners();
    }
}
