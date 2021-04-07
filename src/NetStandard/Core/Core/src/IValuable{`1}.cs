// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public interface IValuable<out T> : INullable<T>
    {
        bool HasNoValue { get; }
        bool HasValue { get; }
    }
}
