using System.Collections.Generic;
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
        public virtual bool CastBeforeEquals(object x, object y) {
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
    }
}
