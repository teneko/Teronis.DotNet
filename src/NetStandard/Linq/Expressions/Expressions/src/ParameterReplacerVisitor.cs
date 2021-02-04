using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    /// <summary>
    /// The Expression visitor compares visiting parameter node
    /// with each source-node by reference and if true, the
    /// visiting node is about to be replaced by target-node.
    /// </summary>
    public class ParameterReplacingVisitor : ReplacingVisitor<ParameterExpression, ParameterExpression>
    {
        public ParameterReplacingVisitor(IReadOnlyList<SourceTargetPair<ParameterExpression, ParameterExpression>> sourceTargetPairs)
            : base(sourceTargetPairs) { }

        public ParameterReplacingVisitor(ParameterExpression source, ParameterExpression target) 
            : base(source, target) { }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (TryReplaceNode(node, out var replacedNode)) {
                return replacedNode;
            }

            return node;
        }
    }
}
