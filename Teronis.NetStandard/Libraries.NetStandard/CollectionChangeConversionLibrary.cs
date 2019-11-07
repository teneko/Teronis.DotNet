using System;

namespace Teronis.Libraries.NetStandard
{
    public class CollectionChangeConversionLibrary
    {
        public static void ThrowChangeActionMismatchException()
            => throw new ArgumentException("The action of the original collection change missmatch the action of the converted change");
    }
}
