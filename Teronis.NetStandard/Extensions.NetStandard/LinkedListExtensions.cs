using System;
using System.Collections.Generic;
using System.Text;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<T> YieldReversedItems<T>(this LinkedList<T> list)
            => LinkedListTools.YieldReversedItems(list);
    }
}
