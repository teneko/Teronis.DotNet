using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    internal class MemberPathEvaluatorVisitor : ExpressionVisitor
    {
        public List<MemberExpression> memberStack;

        public MemberPathEvaluatorVisitor() =>
            memberStack = new List<MemberExpression>();

        protected override Expression VisitMember(MemberExpression node)
        {
            memberStack.Add(node);
            return base.VisitMember(node);
        }

        private bool isContantOrParameterType(ExpressionType nodeType) =>
            nodeType == ExpressionType.Constant || nodeType == ExpressionType.Parameter;

        private void throwIfNotConstantOrParameter(Expression node)
        {
            if (node is null) {
                throw new ArgumentNullException(nameof(node));
            }

            if (!isContantOrParameterType(node.NodeType)) {
                throw new ArgumentException("Source node is not of type constant or parameter.");
            }
        }

        /// <summary>
        /// Evalutes path of member upwards <see cref="MemberExpression.Expression"/>.
        /// </summary>
        /// <param name="memberNode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Source node is no constant or parameter.</exception>
        /// <exception cref="ArgumentException">Source node is no constant or parameter.</exception>
        public MemberPathEvaluation EvaluateMemberPath(MemberExpression memberNode)
        {
            memberNode = memberNode ?? throw new ArgumentNullException(nameof(memberNode));
            memberStack.Clear();
            VisitMember(memberNode);
            var evaluatedMemberStack = memberStack.ToArray();
            var sourceExpression = evaluatedMemberStack[evaluatedMemberStack.Length - 1].Expression;
            throwIfNotConstantOrParameter(sourceExpression);
            return new MemberPathEvaluation(sourceExpression, evaluatedMemberStack);
        }
    }
}
