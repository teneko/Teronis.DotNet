using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class EqualityComparingExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Checks equality for <paramref name="y"/> against visitor.
        /// </summary>
        /// <param name="y">The expression that is used for visitation.</param>
        /// <returns>True if visitor has proven <paramref name="y"/> as equal.</returns>
        public static bool CheckEquality(EqualityComparingExpressionVisitor visitor, Expression y)
        {
            visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            visitor.Visit(y);

            if (visitor.unwindedExpressions.Count > 0) {
                visitor.stopVisitation();
            }

            return visitor.AreExpressionsEqual;
        }

        /// <summary>
        /// Checks equality of <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The expression that will be unwinded.</param>
        /// <param name="y">The expression that is used for visitation.</param>
        /// <returns>True of <paramref name="x"/> and <paramref name="y"/> are equal.</returns>
        public static bool CheckEquality(Expression x, Expression y)
        {
            var visitor = new EqualityComparingExpressionVisitor(x);
            return CheckEquality(visitor, y);
        }

        private readonly UnwindedExpressionQueue unwindedExpressions;
        /// <summary>
        /// The unwinded expression will be used to check equality against visited expression.
        /// </summary>
        protected Expression? CurrentUnwindedExpression { get; private set; }

        public bool AreExpressionsEqual { get; private set; }

        protected EqualityComparingExpressionVisitor(Expression expression)
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

        protected T CurrentUnwindedExpressionCastTo<T>(T _)
                where T : Expression =>
                (T)CurrentUnwindedExpression!;

        public override Expression Visit(Expression expression)
        {
            if (expression == null) {
                // Very first expression might be null.
                return expression!;
            }

            if (!AreExpressionsEqual) {
                return expression;
            }

            CurrentUnwindedExpression = peekNextUnwindedExpression();

            if (!checkNotNull(CurrentUnwindedExpression)) {
                return expression;
            }

            if (!checkSameType(CurrentUnwindedExpression!, expression)) {
                return expression;
            }

            popCurrentUnwindedExpression();
            return base.Visit(expression);
        }

        protected override Expression VisitConstant(ConstantExpression constant)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(constant);

            if (!checkEqual(constant.Value, currentUnwindedExpression.Value)) {
                return constant;
            }

            return base.VisitConstant(constant);
        }

        protected override Expression VisitMember(MemberExpression member)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(member);

            if (!checkEqual(member.Member, currentUnwindedExpression.Member)) {
                return member;
            }

            return base.VisitMember(member);
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCall)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(methodCall);

            if (!checkEqual(methodCall.Method, currentUnwindedExpression.Method)) {
                return methodCall;
            }

            return base.VisitMethodCall(methodCall);
        }

        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(parameter);

            if (!checkEqual(parameter.Name, currentUnwindedExpression.Name)) {
                return parameter;
            }

            return base.VisitParameter(parameter);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression type)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(type);

            if (!checkEqual(type.TypeOperand, currentUnwindedExpression.TypeOperand)) {
                return type;
            }

            return base.VisitTypeBinary(type);
        }

        protected override Expression VisitBinary(BinaryExpression binary)
        {
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(binary);

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
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(unary);

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
            var currentUnwindedExpression = CurrentUnwindedExpressionCastTo(@new);

            if (!checkEqual(@new.Constructor, currentUnwindedExpression.Constructor)) {
                return @new;
            }

            compareList(@new.Members, currentUnwindedExpression.Members);
            return base.VisitNew(@new);
        }
    }
}
