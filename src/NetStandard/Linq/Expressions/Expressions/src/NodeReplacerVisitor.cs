using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class NodeReplacerVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        public NodeReplacerVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node) =>
            node == from ? to : base.Visit(node);
    }
}
