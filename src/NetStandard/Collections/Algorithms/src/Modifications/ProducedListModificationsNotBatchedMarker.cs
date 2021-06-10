// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    /// <summary>
    /// This class is tighly coupled with <see cref="IndexPreferredEnumerator{TItem}.IndexedEnumerator"/>. The purpose is
    /// to mark <see cref="IList"/>/<see cref="IReadOnlyList{T}"/> to be yield-iterator infuenced. This is the case when
    /// a collection modification is yielded and processed and not all occuring collection modifications are batched and
    /// then processed.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    internal abstract class ProducedListModificationsNotBatchedMarker<TItem> : IReadOnlyList<TItem>
    {
        protected ProducedListModificationsNotBatchedMarker() { }

        public abstract TItem this[int index] { get; }

        public abstract int Count { get; }

        public IEnumerator<TItem> GetEnumerator() =>
            throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() =>
            throw new NotSupportedException();

        internal sealed class ReadOnlyList : ProducedListModificationsNotBatchedMarker<TItem>
        {
            private readonly IReadOnlyList<TItem> list;

            public ReadOnlyList(IReadOnlyList<TItem> list) =>
                this.list = list;

            public override TItem this[int index] =>
                list[index];

            public override int Count =>
                list.Count;
        }

        internal sealed class List : ProducedListModificationsNotBatchedMarker<TItem>
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
