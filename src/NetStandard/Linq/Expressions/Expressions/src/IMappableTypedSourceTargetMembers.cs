using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface IMappableTypedSourceTargetMembers<SourceType, TargetType>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="from"/> or <paramref name="to"/> is invalid.</exception>
        TypedSourceTargetMemberMapper<SourceType, TargetType> Map(Expression<Func<SourceType, object?>> from, Expression<Func<TargetType, object?>> to);

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="from"/> or <paramref name="to"/> is invalid.</exception>
        TypedSourceTargetMemberMapper<SourceType, TargetType> Map<SourcePropertyType, TargetPropertyType>(Expression<Func<SourceType, SourcePropertyType>> from, 
            Expression<Func<TargetType, TargetPropertyType>> to);
    }
}
