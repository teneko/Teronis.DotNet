// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public interface IYetNullable<out T> : INullable<T>
    {
        bool IsNull { get; }
        bool IsNotNull { get; }
    }
}
