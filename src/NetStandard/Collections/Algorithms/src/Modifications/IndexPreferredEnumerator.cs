using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    internal class IndexPreferredEnumerator<ItemType> : IEnumerator<ItemType>
    {
        public int CurrentLength { get; private set; }

        private readonly IEnumerator<ItemType> enumerator;

        public IndexPreferredEnumerator(IEnumerable<ItemType> enumerable, Func<int> getLastIndex)
        {
            if (enumerable is YieldIteratorInfluencedReadOnlyList<ItemType> list) {
                enumerator = new IndexedEnumerator(list, getLastIndex);
            } else {
                enumerator = enumerable.GetEnumerator();
            }
        }

        public ItemType Current =>
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

        public class IndexedEnumerator : IEnumerator<ItemType>
        {
            private readonly IReadOnlyList<ItemType> list;
            private readonly Func<int> getLastIndex;

            public ItemType Current { get; private set; }

            public IndexedEnumerator(IReadOnlyList<ItemType> list, Func<int> getLastIndex)
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
