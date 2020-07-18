using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Teronis.Linq.Expressions
{
    public struct MemberPathEvaluation : IEquatable<MemberPathEvaluation>
    {
        internal static MemberPathEvaluation Uninitialized = new MemberPathEvaluation();

        /// <summary>
        /// Represents a constant or parameter expression.
        /// </summary>
        public readonly Expression SourceExpression { get; }
        public readonly bool HasSourceExpression { get; }
        /// <summary>
        /// Member stack ordered from deep (right) to high (left).
        /// </summary>
        public readonly MemberExpression[] MemberStack { get; }
        public readonly bool HasHighMemberExpression { get; }

        public MemberPathEvaluation(Expression sourceExpression, MemberExpression[] memberStack)
        {
            SourceExpression = sourceExpression ?? throw new ArgumentNullException(nameof(sourceExpression));
            HasSourceExpression = true;
            MemberStack = memberStack ?? throw new ArgumentNullException(nameof(memberStack));
            HasHighMemberExpression = (MemberStack != null && MemberStack.Length != 0);
        }

        public Expression? GetHighestExpression()
        {
            if (HasHighMemberExpression) {
                return MemberStack[0];
            }

            return SourceExpression;
        }

        public bool Equals([AllowNull] MemberPathEvaluation otherEvaluation)
        {
            if (!Equals(otherEvaluation, Uninitialized)) {
                bool evaluationEquality;

                if (SourceExpression is ConstantExpression xConstant && otherEvaluation.SourceExpression is ConstantExpression yConstant) {
                    evaluationEquality = EqualityComparer<ConstantExpression>.Default.Equals(xConstant, yConstant);
                } else if (SourceExpression is ParameterExpression xParamater && otherEvaluation.SourceExpression is ParameterExpression yParameter) {
                    evaluationEquality = EqualityComparer<ParameterExpression>.Default.Equals(xParamater, yParameter);
                } else {
                    evaluationEquality = false;
                }

                evaluationEquality = evaluationEquality
                    && Enumerable.SequenceEqual(
                        MemberStack.Select(x => x.Member),
                        otherEvaluation.MemberStack.Select(x => x.Member),
                        EqualityComparer<MemberInfo>.Default);

                return evaluationEquality;
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
