// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Windows.Input
{
    public delegate bool RelayCommandPredicate<in T>([AllowNull]T obj);
}
