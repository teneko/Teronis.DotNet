using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public interface IIndexedEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
