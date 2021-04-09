// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public interface INullable<out T>
    {
        [MaybeNull]
        T Value { get; }
    }
}
