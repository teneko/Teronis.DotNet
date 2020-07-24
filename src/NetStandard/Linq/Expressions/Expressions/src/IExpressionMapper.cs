using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface IExpressionMapper 
    {
        void Map(Expression from, Expression to);
        void MapBody(LambdaExpression from, LambdaExpression to);
    }
}
