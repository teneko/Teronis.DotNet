using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class TypedSourceTargetMemberMappper<SourceType, TargetType>
    {
        private readonly List<MemberPathMapping> memberMappings;

        public TypedSourceTargetMemberMappper() =>
            memberMappings = new List<MemberPathMapping>();

        protected virtual Expression GetBody(Expression<Func<SourceType, object>> from) =>
            from.Body;

        protected virtual Expression GetBody(Expression<Func<TargetType, object>> to) =>
            to.Body;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="from"/> or <paramref name="to"/> is invalid.</exception>
        public TypedSourceTargetMemberMappper<SourceType, TargetType> Map(Expression<Func<SourceType, object>> from, Expression<Func<TargetType, object>> to)
        {
            if (from?.Body is null) {
                throw new ArgumentNullException(nameof(from), $"Body or parameter is null");
            }

            if (to?.Body is null) {
                throw new ArgumentNullException(nameof(to), $"Body or parameter is null");
            }

            var fromBody = GetBody(from) ?? throw new ArgumentNullException($"{nameof(GetBody)}({nameof(from)})", "Body is null.");
            var toBody = GetBody(to) ?? throw new ArgumentNullException($"{nameof(GetBody)}({nameof(to)})", "Body is null.");
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
