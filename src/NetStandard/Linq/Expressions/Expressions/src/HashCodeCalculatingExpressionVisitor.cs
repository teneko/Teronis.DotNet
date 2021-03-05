using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class HashCodeCalculatingExpressionVisitor : ExpressionVisitor
    {
        public static int CalculateHashCode(Expression expression)
        {
            var visitor = new HashCodeCalculatingExpressionVisitor();
            visitor.Visit(expression);
            return visitor.hashCode.ToHashCode();
        }

        private HashCode hashCode;

        protected HashCodeCalculatingExpressionVisitor() { }

        private void AddToHash<T>([AllowNull] T hashableValue)
        {
            if (hashableValue is null) {
                return;
            }

            hashCode.Add(hashableValue);
        }

        private void addEnumerableToHash<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable is null) {
                return;
            }

            foreach (var item in enumerable) {
                AddToHash(item);
            }
        }

        public override Expression Visit(Expression? expression)
        {
            if (expression == null) {
                return expression!; // Might be the very first expression.
            }

            AddToHash(expression.NodeType);
            AddToHash(expression.Type);
            return base.Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression member)
        {
            AddToHash(member.Member);
            return base.VisitMember(member);
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCall)
        {
            AddToHash(methodCall.Method);
            return base.VisitMethodCall(methodCall);
        }

        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            AddToHash(parameter.Name);
            return base.VisitParameter(parameter);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression type)
        {
            AddToHash(type.TypeOperand);
            return base.VisitTypeBinary(type);
        }

        protected override Expression VisitBinary(BinaryExpression binary)
        {
            AddToHash(binary.Method);
            AddToHash(binary.IsLifted);
            AddToHash(binary.IsLiftedToNull);
            return base.VisitBinary(binary);
        }

        protected override Expression VisitConstant(ConstantExpression constant)
        {
            AddToHash(constant.Value);
            return base.VisitConstant(constant);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            addEnumerableToHash(node.Initializers);
            return base.VisitListInit(node);
        }

        protected override Expression VisitUnary(UnaryExpression unary)
        {
            AddToHash(unary.Method);
            AddToHash(unary.IsLifted);
            AddToHash(unary.IsLiftedToNull);
            return base.VisitUnary(unary);
        }

        protected override Expression VisitNew(NewExpression @new)
        {
            AddToHash(@new.Constructor);
            addEnumerableToHash(@new.Members);
            return base.VisitNew(@new);
        }
    }
}
