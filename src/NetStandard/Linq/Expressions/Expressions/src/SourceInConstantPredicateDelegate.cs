// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Linq.Expressions
{
    public delegate bool SourceInConstantPredicateDelegate<SourceType, ComparisonType>([MaybeNull] SourceType source, [MaybeNull] ComparisonType comparisonValue);
}
