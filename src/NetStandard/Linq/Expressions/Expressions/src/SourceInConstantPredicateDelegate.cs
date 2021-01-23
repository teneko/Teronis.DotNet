using System.Diagnostics.CodeAnalysis;

namespace Teronis.Linq.Expressions
{
    public delegate bool SourceInConstantPredicateDelegate<SourceType, ComparisonType>(SourceType source, [MaybeNull] ComparisonType comparisonValue);
}
