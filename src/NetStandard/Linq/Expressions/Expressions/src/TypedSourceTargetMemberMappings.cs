using System.Collections.Generic;

namespace Teronis.Linq.Expressions
{
    public sealed class TypedSourceTargetMemberMappings
    {
        public IReadOnlyCollection<MemberPathMapping> MemberMappings { get; }

        internal TypedSourceTargetMemberMappings(IReadOnlyCollection<MemberPathMapping> memberMappings)
        {
            MemberMappings = memberMappings ?? throw new System.ArgumentNullException(nameof(memberMappings));
        }
    }
}
