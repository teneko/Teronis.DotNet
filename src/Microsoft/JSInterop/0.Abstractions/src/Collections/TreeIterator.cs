// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public abstract class TreeIterator<T, TEntry, TEnumerator> : ITreeIterator<TEntry>
        where TEntry : class, ITreeIteratorEntry
        where TEnumerator : class, ITreeIteratorEnumerator<TEntry>
    {
        public TEntry Entry =>
            Enumerator.Current ?? throw new InvalidOperationException("The entry is not available.");

        public IReadOnlyList<TEntry> Entries { get; }

        public TEnumerator Enumerator {
            get {
                if (enumerator is null) {
                    enumerator = CreateEnumerator(Entries);
                }

                return enumerator;
            }
        }

        IEnumerator<TEntry> ITreeIterator<TEntry>.Enumerator =>
            Enumerator;

        public bool StopRequested { get; private set; }

        private TEnumerator? enumerator;

        protected TreeIterator(T item) =>
            Entries = new TEntry[] { CreateEntry(item) };

        protected TreeIterator(TreeIterator<T, TEntry, TEnumerator> context)
        {
            Entries = context.Entries;
            enumerator = CreateEnumerator(Entries, context.Enumerator.CurrentIndex);
        }

        protected TreeIterator(IEnumerable<T> items, int startIndex)
        {
            if (items is null) {
                throw new ArgumentNullException(nameof(items));
            }

            Entries = CreateEntryList(items);
            enumerator = CreateEnumerator(Entries, startIndex - 1);
        }

        protected TreeIterator(IEnumerable<T> items)
        {
            if (items is null) {
                throw new ArgumentNullException(nameof(items));
            }

            Entries = CreateEntryList(items);
            enumerator = CreateEnumerator(Entries);
        }

        protected abstract TEntry CreateEntry(T item);

        protected List<TEntry> CreateEntryList(IEnumerable<T> interceptors) =>
            new List<TEntry>(interceptors.Select(x => CreateEntry(x)));

        protected abstract TEnumerator CreateEnumerator(IReadOnlyList<TEntry> entries, int initialIndex = -1);

        public void StopInterception() =>
            StopRequested = true;
    }
}
