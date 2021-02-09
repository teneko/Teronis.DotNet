using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public abstract class ReplacingVisitor<SourceType, TargetType> : ExpressionVisitor
        where SourceType : Expression
        where TargetType : Expression
    {
        public IReadOnlyList<SourceTargetPair<SourceType, TargetType>> ReplacedSourceTargetPairs =>
            replacedSourceTargetPairs;

        private readonly IReadOnlyList<SourceTargetPair<SourceType, TargetType>> sourceTargetPairs;
        private readonly List<SourceTargetPair<SourceType, TargetType>> replacedSourceTargetPairs;

        public ReplacingVisitor
            (IReadOnlyList<SourceTargetPair<SourceType, TargetType>> sourceTargetPairs)
        {
            replacedSourceTargetPairs = new List<SourceTargetPair<SourceType, TargetType>>();
            this.sourceTargetPairs = sourceTargetPairs ?? throw new ArgumentNullException(nameof(sourceTargetPairs));
        }

        public ReplacingVisitor(SourceType source, TargetType target)
            : this(new[] { new SourceTargetPair<SourceType, TargetType>(source, target) }) { }

        /// <summary>
        /// You need to call this explicity.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The node that got passed or if weighted as replacable the target node.</returns>
        protected bool TryReplaceNode(Expression node, [MaybeNullWhen(false)] out TargetType replacedNode)
        {
            for (int i = 0; i < sourceTargetPairs.Count; i++) {
                var sourceTargetPair = sourceTargetPairs[i];

                if (ReferenceEquals(node, sourceTargetPair.Source)) {
                    replacedSourceTargetPairs.Add(sourceTargetPair);
                    replacedNode = sourceTargetPair.Target;
                    return true;
                }
            }

            replacedNode = null;
            return false;
        }
    }
}
