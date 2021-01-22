using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public abstract class YieldIteratorInfluencedReadOnlyList<ItemType> : IReadOnlyList<ItemType>
    {
        protected YieldIteratorInfluencedReadOnlyList() { }

        public abstract ItemType this[int index] { get; }

        public abstract int Count { get; }

        public IEnumerator<ItemType> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();

        internal sealed class ReadOnlyList : YieldIteratorInfluencedReadOnlyList<ItemType>
        {
            private readonly IReadOnlyList<ItemType> list;

            public ReadOnlyList(IReadOnlyList<ItemType> list) =>
                this.list = list;

            public override ItemType this[int index] =>
                list[index];

            public override int Count =>
                list.Count;
        }

        internal sealed class List : YieldIteratorInfluencedReadOnlyList<ItemType>
        {
            private readonly IList<ItemType> list;

            public List(IList<ItemType> list) =>
                this.list = list;

            public override ItemType this[int index] =>
                list[index];

            public override int Count =>
                list.Count;
        }
    }
}
