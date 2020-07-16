using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public class SuccesssivKeysComparer<TKey1, TKey2> : Comparer<SuccesssivKeys<TKey1, TKey2>>, IComparer<SuccesssivKeys<TKey1, TKey2>?>
    {
        public new static SuccesssivKeysComparer<TKey1, TKey2> Default = new SuccesssivKeysComparer<TKey1, TKey2>();

        public override int Compare([AllowNull] SuccesssivKeys<TKey1, TKey2> x, [AllowNull] SuccesssivKeys<TKey1, TKey2> y) =>
            new IOrderedKeysProviderComparer().Compare(x, y);

        public int Compare([AllowNull] SuccesssivKeys<TKey1, TKey2>? x, [AllowNull] SuccesssivKeys<TKey1, TKey2>? y) =>
            new IOrderedKeysProviderComparer().Compare(x, y);
    }
}
