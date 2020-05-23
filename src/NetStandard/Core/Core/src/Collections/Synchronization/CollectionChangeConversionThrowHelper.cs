using System;

namespace Teronis.Collections.Synchronization
{
    internal static class CollectionChangeConversionThrowHelper
    {
        public static void ThrowChangeActionMismatchException()
            => throw new ArgumentException("The action of the original collection change missmatch the action of the converted change");
    }
}
