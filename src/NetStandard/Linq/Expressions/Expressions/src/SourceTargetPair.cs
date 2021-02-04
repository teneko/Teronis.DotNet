using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct SourceTargetPair<SourceType, TargetType>
        where SourceType : Expression
        where TargetType : Expression
    {
        public SourceType Source { get; }
        public TargetType Target { get; }

        public SourceTargetPair(SourceType source, TargetType target)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }
    }
}
