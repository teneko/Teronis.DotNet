// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class EqualityComparingExpressionReplacerVisitor : ExpressionVisitor
    {
        public IReadOnlyCollection<ExpressionMapping> expressionMappings;

        public EqualityComparingExpressionReplacerVisitor(IReadOnlyCollection<ExpressionMapping> expressionMappings) =>
            this.expressionMappings = expressionMappings;

        public override Expression Visit(Expression? node)
        {
            if (node is null || expressionMappings.Count == 0) {
                return node!;
            }

            foreach (var expressionMapping in expressionMappings) {
                if (ExpressionEqualityComparer.Default.Equals(node, expressionMapping.SourceExpression)) {
                    return expressionMapping.TargetExpression;
                }
            }

            return base.Visit(node);
        }
    }
}
