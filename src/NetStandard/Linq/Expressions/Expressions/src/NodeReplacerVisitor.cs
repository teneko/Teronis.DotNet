using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    /// <summary>
    /// The Expression visitor compares visiting node with
    /// each source-node by reference and if true, the visiting
    /// node is about to be replaced by target-node.
    /// </summary>
    public class NodeReplacerVisitor : ReplacingVisitor<Expression, Expression>
    {
        public NodeReplacerVisitor(IReadOnlyList<SourceTargetPair<Expression, Expression>> sourceTargetPairs)
            : base(sourceTargetPairs) { }

        public NodeReplacerVisitor(Expression source, Expression target)
            : base(source, target) { }

        public override Expression Visit(Expression node)
        {
            if (TryReplaceNode(node, out var replacedNode)) {
                return replacedNode;
            }

            return node;
        }
    }
}
