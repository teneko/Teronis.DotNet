// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    internal abstract class YieldIteratorInfluencedReadOnlyList<TItem> : IReadOnlyList<TItem>
    {
        protected YieldIteratorInfluencedReadOnlyList() { }

        public abstract TItem this[int index] { get; }

        public abstract int Count { get; }

        public IEnumerator<TItem> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();

        internal sealed class ReadOnlyList : YieldIteratorInfluencedReadOnlyList<TItem>
        {
            private readonly IReadOnlyList<TItem> list;

            public ReadOnlyList(IReadOnlyList<TItem> list) =>
                this.list = list;

            public override TItem this[int index] =>
                list[index];

            public override int Count =>
                list.Count;
        }

        internal sealed class List : YieldIteratorInfluencedReadOnlyList<TItem>
        {
            private readonly IList<TItem> list;

            public List(IList<TItem> list) =>
                this.list = list;

            public override TItem this[int index] =>
                list[index];

            public override int Count =>
                list.Count;
        }
    }
}
