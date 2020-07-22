using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Teronis.Linq.Expressions.Utils;

namespace Teronis.Linq.Expressions
{
    public class TypedSourceTargetMemberMapper<SourceType, TargetType> : ITypedSourceTargetMemberMapper<SourceType, TargetType>
    {
        private readonly List<MemberPathMapping> memberMappings;

        public TypedSourceTargetMemberMapper() =>
            memberMappings = new List<MemberPathMapping>();

        protected virtual MemberExpression GetMemberMappingFrom(LambdaExpression from) =>
            ExpressionUtils.TryGetMember(from.Body)!;

        protected virtual MemberExpression GetMemberMappingTo(LambdaExpression to) =>
            ExpressionUtils.TryGetMember(to.Body)!;

        /// <inheritdoc/>
        public TypedSourceTargetMemberMapper<SourceType, TargetType> Map(Expression<Func<SourceType, object?>> from, Expression<Func<TargetType, object?>> to)
        {
            var fromBody = GetMemberMappingFrom(from) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var toBody = GetMemberMappingTo(to) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var memberMapping = MemberPathMapping.Create(fromBody, toBody);
            memberMappings.Add(memberMapping);
            return this;
        }

        /// <inheritdoc/>
        public TypedSourceTargetMemberMapper<SourceType, TargetType> Map<SourcePropertyType, TargetPropertyType>(Expression<Func<SourceType, SourcePropertyType>> from,
            Expression<Func<TargetType, TargetPropertyType>> to)
        {
            var fromBody = GetMemberMappingFrom(from) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var toBody = GetMemberMappingTo(to) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var memberMapping = MemberPathMapping.Create(fromBody, toBody);
            memberMappings.Add(memberMapping);
            return this;
        }

        /// <summary>
        /// Provides a collection of mapped members.
        /// </summary>
        /// <param name="copyList"></param>
        /// <returns>New created collection of current mapped members.</returns>
        public IEnumerable<MemberPathMapping> GetMappings() =>
            new ReadOnlyCollection<MemberPathMapping>(memberMappings);
    }
}
