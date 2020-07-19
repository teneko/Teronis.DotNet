namespace Teronis.EntityFrameworkCore.Query
{
    internal interface IChildCollectionConstantPredicateBuilder
    {
        void AppendConcatenatedExpressionToParent();
    }
}
