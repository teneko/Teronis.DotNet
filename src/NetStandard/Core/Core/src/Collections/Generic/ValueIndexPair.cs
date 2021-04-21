// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Generic
{
    public struct ValueIndexPair<T>
    {
        public T Value { get; private set; }
        public int Index { get; private set; }

        public ValueIndexPair(T value, int index)
        {
            Value = value;
            Index = index;
        }

        public override string ToString() => $"[{Value}, {Index}]";
    }
}
