using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    internal readonly struct SourceValueComparison<ComparisonType>
    {
        public readonly ComparisonType ComparisonValue;
        public readonly Expression SourcePredicate;

        public SourceValueComparison(ComparisonType comparisonValue, Expression sourcePredicate)
        {
            ComparisonValue = comparisonValue;
            SourcePredicate = sourcePredicate;
        }
    }
}
