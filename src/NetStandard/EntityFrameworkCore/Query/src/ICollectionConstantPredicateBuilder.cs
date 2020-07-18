namespace Teronis.EntityFrameworkCore.Query
{
    public interface ICollectionConstantPredicateBuilder
    {
        void AppendConcatenatedExpressionToParent();
    }
}
