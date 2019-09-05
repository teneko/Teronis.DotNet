using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Libraries.NetStandard
{
    public class CollectionChangeConversionLibrary
    {
        public static void ThrowActionMismatchException()
            => throw new ArgumentException("The action of the original collection change missmatch the action of the converted change");
    }
}
