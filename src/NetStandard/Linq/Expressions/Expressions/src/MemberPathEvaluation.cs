using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public struct MemberPathEvaluation : IEquatable<MemberPathEvaluation>
    {
        /// <summary>
        /// Represents a constant or parameter expression.
        /// </summary>
        public readonly Expression SourceExpression { get; }
        public readonly bool HasSourceExpression { get; }
        /// <summary>
        /// Member stack ordered from deep (right) to high (left).
        /// </summary>
        public readonly MemberExpression[] MemberStack { get; }
        public readonly bool HasAscendantMemberExpression { get; }

        public MemberPathEvaluation(Expression sourceExpression, MemberExpression[] memberStack)
        {
            SourceExpression = sourceExpression ?? throw new ArgumentNullException(nameof(sourceExpression));
            HasSourceExpression = true;
            MemberStack = memberStack ?? throw new ArgumentNullException(nameof(memberStack));
            HasAscendantMemberExpression = (MemberStack != null && MemberStack.Length != 0);
        }

        /// <summary>
        /// Gets the highest expression. This can be the first item of 
        /// <see cref="MemberStack"/> or <see cref="SourceExpression"/>.
        /// When one or the other is not there, null is returned.
        /// </summary>
        /// <returns>The highest expression or null.</returns>
        public Expression? GetMostAscendantExpression()
        {
            if (HasAscendantMemberExpression) {
                return MemberStack[0];
            }

            return SourceExpression;
        }

        public bool Equals([AllowNull] MemberPathEvaluation otherEvaluation)
        {
            bool areSourceExpressionEqual;

            if (SourceExpression is ConstantExpression xConstant && otherEvaluation.SourceExpression is ConstantExpression yConstant) {
                areSourceExpressionEqual = EqualityComparer<ConstantExpression>.Default.Equals(xConstant, yConstant);
            } else if (SourceExpression is ParameterExpression xParamater && otherEvaluation.SourceExpression is ParameterExpression yParameter) {
                areSourceExpressionEqual = EqualityComparer<ParameterExpression>.Default.Equals(xParamater, yParameter);
            } else {
                return false;
            }

            if (areSourceExpressionEqual) {
                var memberStackEnumerator = MemberStack.GetEnumerator();
                var otherMemberStackEnumerator = otherEvaluation.MemberStack.GetEnumerator();
                bool memberStackHasNext, otherMemberStackHasNext;

                while ((memberStackHasNext = memberStackEnumerator.MoveNext())
                        & (otherMemberStackHasNext = otherMemberStackEnumerator.MoveNext())) {
                    var currentValue = memberStackEnumerator.Current;
                    var otherCurrentValue = otherMemberStackEnumerator.Current;

                    if (!ExpressionEqualityComparer.Default.CastableEquals(currentValue, otherCurrentValue)) {
                        return false;
                    }
                }

                if (!memberStackHasNext && !otherMemberStackHasNext) {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is MemberPathEvaluation otherEvaluation) {
                return Equals(otherEvaluation);
            }

            return false;
        }

        public override int GetHashCode() =>
            HashCode.Combine(SourceExpression, MemberStack);
    }
}
