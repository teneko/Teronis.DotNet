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

        protected virtual MemberExpression GetBody(Expression<Func<SourceType, object?>> from) =>
            (MemberExpression)from.Body;

        protected virtual MemberExpression GetBody(Expression<Func<TargetType, object?>> to) =>
            (MemberExpression)to.Body;

        /// <inheritdoc/>
        public TypedSourceTargetMemberMapper<SourceType, TargetType> Map(Expression<Func<SourceType, object?>> from, Expression<Func<TargetType, object?>> to)
        {
            MemberPathMapping.ThrowOnNonMemberBody(from, nameof(from));
            MemberPathMapping.ThrowOnNonMemberBody(to, nameof(to));
            var fromBody = GetBody(from) ?? throw new ArgumentNullException($"{nameof(GetBody)}({nameof(from)})", "Member expression is null.");
            var toBody = GetBody(to) ?? throw new ArgumentNullException($"{nameof(GetBody)}({nameof(to)})", "Member expression is null.");
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
