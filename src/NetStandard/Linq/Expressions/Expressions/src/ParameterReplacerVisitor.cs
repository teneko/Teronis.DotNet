using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyList<ParameterExpression> from, to;

        public ParameterReplacerVisitor(IReadOnlyList<ParameterExpression> from,
            IReadOnlyList<ParameterExpression> to)
        {
            this.from = from ?? throw new ArgumentNullException(nameof(from));
            this.to = to ?? throw new ArgumentNullException(nameof(to));

            if (from.Count != to.Count) {
                throw new InvalidOperationException("Parameter expression lengths have to be equal.");
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            for (int i = 0; i < from.Count; i++) {
                if (node == from[i]) {
                    return to[i];
                }
            }

            return node;
        }
    }
}
