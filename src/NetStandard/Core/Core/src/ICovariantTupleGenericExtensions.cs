// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public static class ICovariantTupleGenericExtensions
    {
        public static void Deconstruct<T1, T2>(this ICovariantTuple<T1, T2> tuple, out T1 item1, out T2 item2) {
            item1 = tuple.Item1;
            item2 = tuple.Item2;
        }
    }
}
