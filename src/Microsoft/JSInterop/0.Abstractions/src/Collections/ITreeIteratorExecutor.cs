// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public interface ITreeIteratorExecutor<TEntry>
    {
        ValueTask ExecuteIteratorAsync(ITreeIterator<TEntry> iterator, Func<TEntry, ValueTask> handler);
    }
}