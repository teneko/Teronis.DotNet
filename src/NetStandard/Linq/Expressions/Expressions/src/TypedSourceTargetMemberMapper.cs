using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class TypedSourceTargetMemberMapper<SourceType, TargetType> : IMappableTypedSourceTargetMembers<SourceType, TargetType>
    {
        private readonly List<MemberPathMapping> memberMappings;

        public TypedSourceTargetMemberMapper() =>
            memberMappings = new List<MemberPathMapping>();

        protected virtual MemberExpression GetMappingFromMember(LambdaExpression from) =>
            (MemberExpression)from.Body;

        protected virtual MemberExpression GetMappingToMember(LambdaExpression to) =>
            (MemberExpression)to.Body;

        /// <inheritdoc/>
        public TypedSourceTargetMemberMapper<SourceType, TargetType> Map(Expression<Func<SourceType, object?>> from, Expression<Func<TargetType, object?>> to)
        {
            var fromBody = GetMappingFromMember(from) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var toBody = GetMappingToMember(to) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var memberMapping = MemberPathMapping.Create(fromBody, toBody);
            memberMappings.Add(memberMapping);
            return this;
        }

        /// <inheritdoc/>
        public TypedSourceTargetMemberMapper<SourceType, TargetType> Map<SourcePropertyType, TargetPropertyType>(Expression<Func<SourceType, SourcePropertyType>> from,
            Expression<Func<TargetType, TargetPropertyType>> to)
        {
            var fromBody = GetMappingFromMember(from) ?? throw new InvalidOperationException("Member expression is invalid (null).");
            var toBody = GetMappingToMember(to) ?? throw new InvalidOperationException("Member expression is invalid (null).");
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
