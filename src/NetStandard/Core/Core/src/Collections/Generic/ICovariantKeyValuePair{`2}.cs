// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Generic
{
    public interface ICovariantKeyValuePair<out KeyType, out ValueType>
    {
        KeyType Key { get; }
        ValueType Value { get; }
    }
}
