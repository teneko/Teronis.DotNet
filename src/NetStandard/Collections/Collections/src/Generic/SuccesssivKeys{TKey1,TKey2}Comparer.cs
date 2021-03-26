// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public class SuccessivKeysComparer<TKey1, TKey2> : Comparer<SuccessivKeys<TKey1, TKey2>>, IComparer<SuccessivKeys<TKey1, TKey2>?>
    {
        public new static SuccessivKeysComparer<TKey1, TKey2> Default = new SuccessivKeysComparer<TKey1, TKey2>();

        public override int Compare([AllowNull] SuccessivKeys<TKey1, TKey2> x, [AllowNull] SuccessivKeys<TKey1, TKey2> y) =>
            new IOrderedKeysProviderComparer().Compare(x, y);

        public int Compare([AllowNull] SuccessivKeys<TKey1, TKey2>? x, [AllowNull] SuccessivKeys<TKey1, TKey2>? y) =>
            new IOrderedKeysProviderComparer().Compare(x, y);
    }
}
