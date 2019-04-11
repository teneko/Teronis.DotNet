using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public static class ShiftTools
    {
        public static void Shift<S, T>(int fromIndex, int toIndex, Func<int, T> getAt, Action<int, T> insert, Action<int> removeAt)
        {
            var from = getAt(fromIndex);
            insert(fromIndex < toIndex ? toIndex + 1 : toIndex, from);
            removeAt(toIndex < fromIndex ? fromIndex + 1 : fromIndex);
        }
    }
}
