using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public class IListTools
    {
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = ThreadSafeRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
