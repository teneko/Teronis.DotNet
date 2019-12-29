using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    internal class CovariantList<T> : IEnumerable<T>, IIndexedEnumerable<T>
    {
        private readonly IList<T> list;

        public CovariantList(IList<T> list) => this.list = list;

        public T this[int index] { get { return list[index]; } }
        public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
        public int Count { get { return list.Count; } }
    }
}
