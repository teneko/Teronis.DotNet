using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public class Wrapper<T>
    {
        public T Item { get; set; }

        public Wrapper(T item) => Item = item;
    }
}
