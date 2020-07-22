using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class ExpressionEqualityComparer : EqualityComparer<Expression>
    {
        public new static ExpressionEqualityComparer Default = new ExpressionEqualityComparer();

        /// <summary>
        /// Takes the following into account when checking for equality:
        /// <br />Expression VisitConstant(ConstantExpression constant);
        /// <br />Expression VisitMember(MemberExpression member);
        /// <br />Expression VisitMethodCall(MethodCallExpression methodCall);
        /// <br />Expression VisitParameter(ParameterExpression parameter);
        /// <br />Expression VisitTypeBinary(TypeBinaryExpression type);
        /// <br />Expression VisitBinary(BinaryExpression binary;
        /// <br />Expression VisitUnary(UnaryExpression unary);
        /// <br />Expression VisitNew(NewExpression @new);
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if both expressions are equal.</returns>
        public override bool Equals(Expression x, Expression y) =>
            EqualityComparingExpressionVisitor.CheckEquality(x, y);

        /// <summary>
        /// Checks for equality. If no null is involved the parameters are 
        /// tried to be casted to <see cref="Expression"/> and checked against
        /// <see cref="Equals(Expression, Expression)"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if both objects are equal.</returns>
        public bool CastableEquals(object x, object y) {
            if (x is null && y is null) {
                return true;
            } else if (x is null || y is null) {
                return false;
            }

            if (x is Expression expression && y is Expression otherExpression) {
                return Equals(expression, otherExpression);
            }

            return false;
        }

        /// <summary>
        /// Takes the following into account when calculating the hash code:
        /// <br />Expression VisitMember(MemberExpression member);
        /// <br />Expression VisitMethodCall(MethodCallExpression methodCall);
        /// <br />Expression VisitParameter(ParameterExpression parameter);
        /// <br />Expression VisitTypeBinary(TypeBinaryExpression type);
        /// <br />Expression VisitBinary(BinaryExpression binary;
        /// <br />Expression VisitConstant(ConstantExpression constant);
        /// <br />Expression VisitListInit(ListInitExpression node)
        /// <br />Expression VisitUnary(UnaryExpression unary);
        /// <br />Expression VisitNew(NewExpression @new);
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override int GetHashCode(Expression expression) =>
            HashCodeCalculatingExpressionVisitor.CalculateHashCode(expression);

        private class EqualityComparingExpressionVisitor : ExpressionVisitor
        {
            public static bool CheckEquality(Expression x, Expression y)
            {
                var visitor = new EqualityComparingExpressionVisitor(x);
                visitor.Visit(y);

                if (visitor.unwindedExpressions.Count > 0) {
                    visitor.stopVisitation();
                }

                return visitor.AreExpressionsEqual;
            }

            private readonly UnwindedExpressionQueue unwindedExpressions;
            private Expression? currentUnwindedExpression;

            public bool AreExpressionsEqual { get; private set; }

            private EqualityComparingExpressionVisitor(Expression expression)
            {
                AreExpressionsEqual = true;
                unwindedExpressions = new UnwindedExpressionQueue(expression);
            }

            private void stopVisitation() =>
                AreExpressionsEqual = false;

            private Expression? peekNextUnwindedExpression()
            {
                if (unwindedExpressions.Count == 0) {
                    return null;
                }

                return unwindedExpressions.Peek();
            }

            private Expression popCurrentUnwindedExpression() =>
                unwindedExpressions.Dequeue();

            private bool checkEqual<T>(T expression, T otherExpression)
            {
                if (!EqualityComparer<T>.Default.Equals(expression, otherExpression)) {
                    stopVisitation();
                    return false;
                }

                return true;
            }

            private bool checkSameType(Expression expression, Expression otherExpression)
            {
                if (!checkEqual(expression.NodeType, otherExpression.NodeType)) {
                    return false;
                } else if (!checkEqual(expression.Type, otherExpression.Type)) {
                    return false;
                }

                return true;
            }

            private void compareList<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> otherCollection, Func<T, T, bool> comparer)
            {
                if (!checkSameLength(collection, otherCollection)) {
                    return;
                }

                for (int i = 0; i < collection.Count; i++) {
                    if (!comparer(collection[i], otherCollection[i])) {
                        stopVisitation();
                        return;
                    }
                }
            }

            private void compareList<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> otherCollection) =>
                compareList(collection, otherCollection, (item, currentUnwindedExpression) =>
                    EqualityComparer<T>.Default.Equals(item, currentUnwindedExpression));

            private bool checkSameLength<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> otherCollection) =>
                checkEqual(collection.Count, otherCollection.Count);

            private bool checkNotNull<T>([AllowNull] T value)
            {
                if (value is null) {
                    stopVisitation();
                    return false;
                }

                return true;
            }

            private T currentUnwindedExpressionCastTo<T>(T _)
                    where T : Expression =>
                    (T)currentUnwindedExpression!;

            public override Expression Visit(Expression expression)
            {
                if (expression == null) {
                    // Very first expression might be null.
                    return expression!;
                }

                if (!AreExpressionsEqual) {
                    return expression;
                }

                currentUnwindedExpression = peekNextUnwindedExpression();

                if (!checkNotNull(currentUnwindedExpression)) {
                    return expression;
                }

                if (!checkSameType(currentUnwindedExpression!, expression)) {
                    return expression;
                }

                popCurrentUnwindedExpression();
                return base.Visit(expression);
            }

            protected override Expression VisitConstant(ConstantExpression constant)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(constant);

                if (!checkEqual(constant.Value, currentUnwindedExpression.Value)) {
                    return constant;
                }

                return base.VisitConstant(constant);
            }

            protected override Expression VisitMember(MemberExpression member)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(member);

                if (!checkEqual(member.Member, currentUnwindedExpression.Member)) {
                    return member;
                }

                return base.VisitMember(member);
            }

            protected override Expression VisitMethodCall(MethodCallExpression methodCall)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(methodCall);

                if (!checkEqual(methodCall.Method, currentUnwindedExpression.Method)) {
                    return methodCall;
                }

                return base.VisitMethodCall(methodCall);
            }

            protected override Expression VisitParameter(ParameterExpression parameter)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(parameter);

                if (!checkEqual(parameter.Name, currentUnwindedExpression.Name)) {
                    return parameter;
                }

                return base.VisitParameter(parameter);
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression type)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(type);

                if (!checkEqual(type.TypeOperand, currentUnwindedExpression.TypeOperand)) {
                    return type;
                }

                return base.VisitTypeBinary(type);
            }

            protected override Expression VisitBinary(BinaryExpression binary)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(binary);

                if (!checkEqual(binary.Method, currentUnwindedExpression.Method)) {
                    return binary;
                } else if (!checkEqual(binary.IsLifted, currentUnwindedExpression.IsLifted)) {
                    return binary;
                } else if (!checkEqual(binary.IsLiftedToNull, currentUnwindedExpression.IsLiftedToNull)) {
                    return binary!;
                }

                return base.VisitBinary(binary);
            }

            protected override Expression VisitUnary(UnaryExpression unary)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(unary);

                if (!checkEqual(unary.Method, currentUnwindedExpression.Method)) {
                    return unary;
                }

                if (!checkEqual(unary.IsLifted, currentUnwindedExpression.IsLifted)) {
                    return unary;
                }

                if (!checkEqual(unary.IsLiftedToNull, currentUnwindedExpression.IsLiftedToNull)) {
                    return unary;
                }

                return base.VisitUnary(unary);
            }

            protected override Expression VisitNew(NewExpression @new)
            {
                var currentUnwindedExpression = currentUnwindedExpressionCastTo(@new);

                if (!checkEqual(@new.Constructor, currentUnwindedExpression.Constructor)) {
                    return @new;
                }

                compareList(@new.Members, currentUnwindedExpression.Members);
                return base.VisitNew(@new);
            }
        }

        private class HashCodeCalculatingExpressionVisitor : ExpressionVisitor
        {
            public static int CalculateHashCode(Expression expression)
            {
                var visitor = new HashCodeCalculatingExpressionVisitor();
                visitor.Visit(expression);
                return visitor.hashCode.ToHashCode();
            }

            private HashCode hashCode;

            private HashCodeCalculatingExpressionVisitor() { }

            private void AddToHash<T>(T hashableValue) =>
                hashCode.Add(hashableValue);

            private void addEnumerableToHash<T>(IEnumerable<T> enumerable)
            {
                foreach (var item in enumerable) {
                    AddToHash(item);
                }
            }

            public override Expression Visit(Expression expression)
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
                AddToHash(constant?.Value);
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
                AddToHash(@new.Constructor.GetHashCode());
                addEnumerableToHash(@new.Members);
                return base.VisitNew(@new);
            }
        }
    }
}
