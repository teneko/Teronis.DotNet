using System.Collections.Generic;

namespace Teronis.Linq.Expressions
{
    public sealed class SourceMemberTargetMemberMappings
    {
        public IReadOnlyCollection<MemberPathMapping> MemberMappings { get; }

        internal SourceMemberTargetMemberMappings(IReadOnlyCollection<MemberPathMapping> memberMappings) {
            MemberMappings = memberMappings ?? throw new System.ArgumentNullException(nameof(memberMappings));
        }
    }
}
