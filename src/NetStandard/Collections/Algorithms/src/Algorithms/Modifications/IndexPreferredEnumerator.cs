// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    internal class IndexPreferredEnumerator<TItem> : IEnumerator<TItem>
    {
        public int CurrentLength { get; private set; }

        private readonly IEnumerator<TItem> enumerator;

        public IndexPreferredEnumerator(IEnumerable<TItem> enumerable, Func<int> getLastIndex)
        {
            if (enumerable is ProducedListModificationsNotBatchedMarker<TItem> list) {
                enumerator = new IndexedEnumerator(list, getLastIndex);
            } else {
                enumerator = enumerable.GetEnumerator();
            }
        }

        public TItem Current =>
            enumerator.Current;

        public bool MoveNext()
        {
            if (enumerator.MoveNext()) {
                CurrentLength++;
                return true;
            } else {
                return false;
            }
        }

        public void Reset() =>
            enumerator.Reset();

        object? IEnumerator.Current =>
            ((IEnumerator)enumerator).Current;

        public void Dispose() =>
            enumerator.Dispose();

        public class IndexedEnumerator : IEnumerator<TItem>
        {
            private readonly IReadOnlyList<TItem> list;
            private readonly Func<int> getLastIndex;

            public TItem Current { get; private set; }

            public IndexedEnumerator(IReadOnlyList<TItem> list, Func<int> getLastIndex)
            {
                Current = default!;
                this.list = list;
                this.getLastIndex = getLastIndex;
            }

            public bool MoveNext()
            {
                var lastIndex = getLastIndex();

                if (lastIndex + 1 < list.Count) {
                    Current = list[lastIndex + 1];
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset() =>
                throw new NotImplementedException();

            object IEnumerator.Current =>
                Current!;

            public void Dispose() { }
        }
    }
}
