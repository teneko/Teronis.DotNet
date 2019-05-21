using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Extensions.NetStandard
{
    public static class ListExtensions
    {
        public static List<T> ReturnedReverse<T>(this List<T> list)
        {
            list.Reverse();
            return list;
        }
    }
}
