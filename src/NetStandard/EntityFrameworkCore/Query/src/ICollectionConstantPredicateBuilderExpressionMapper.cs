using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType> : IParameterReplacableExpressionMapper<SourceType, TargetType>
    {}
}
