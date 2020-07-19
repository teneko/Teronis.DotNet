namespace Teronis.EntityFrameworkCore.Query
{
    internal interface ICollectionConstantPredicateBuilder
    {
        void AppendConcatenatedExpressionToParent();
    }
}
