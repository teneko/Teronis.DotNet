using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class MemberMappingBuilder<SourceType, TargetType>
    {
        private List<MemberPathMapping> memberMappings;

        public MemberMappingBuilder() =>
            memberMappings = new List<MemberPathMapping>();

        public MemberMappingBuilder<SourceType, TargetType> Map(Expression<Func<SourceType, object>> from, Expression<Func<SourceType, object>> to)
        {
            var replacerVisitedFrom = from.Body;
            var replacerVisitedTo = to.Body;
            var memberMapping = MemberPathMapping.Create(replacerVisitedFrom, replacerVisitedTo);
            memberMappings.Add(memberMapping);
            return this;
        }

        public SourceMemberTargetMemberMappings Build() =>
            new SourceMemberTargetMemberMappings(memberMappings);
    }
}
