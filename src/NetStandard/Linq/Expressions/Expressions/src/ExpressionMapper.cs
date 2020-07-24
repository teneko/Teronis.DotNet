using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class ExpressionMapper : IExpressionMapper
    {
        private readonly List<ExpressionMapping> expressionMappings;

        public ExpressionMapper() =>
            expressionMappings = new List<ExpressionMapping>();

        internal void AddMapping(Expression source, Expression replacment) =>
            expressionMappings.Add(new ExpressionMapping(source, replacment));

        public virtual void Map(Expression source, Expression replacement)
        {
            AddMapping(source, replacement);
            return;
        }

        public void MapBody(LambdaExpression source, LambdaExpression replacment)
        {
            Expression sourceBody = source.Body;
            Expression replacmentBody = replacment.Body;
            AddMapping(sourceBody, replacmentBody);
        }

        /// <summary>
        /// Provides a collection of mapped members.
        /// </summary>
        /// <param name="copyList"></param>
        /// <returns>New created collection of current mapped members.</returns>
        public IReadOnlyCollection<ExpressionMapping> GetMappings() =>
            new ReadOnlyCollection<ExpressionMapping>(expressionMappings);
    }
}
