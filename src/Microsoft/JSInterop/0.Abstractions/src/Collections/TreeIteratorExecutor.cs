// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public class TreeIteratorExecutor<TEntry>
    {
        public static TreeIteratorExecutor<TEntry> Default = new TreeIteratorExecutor<TEntry>();

        protected virtual ValueTask HandleEntryAsync(TEntry entry, Func<TEntry, ValueTask> handler) =>
            handler(entry);

        public async ValueTask ExecuteIteratorAsync(ITreeIterator<TEntry> iterator, Func<TEntry, ValueTask> handler)
        {
            var entryEnumerator = iterator.Enumerator;

            while (entryEnumerator.MoveNext()) {
                await HandleEntryAsync(entryEnumerator.Current, handler);

                if (iterator.StopRequested) {
                    return;
                }
            }
        }
    }
}
