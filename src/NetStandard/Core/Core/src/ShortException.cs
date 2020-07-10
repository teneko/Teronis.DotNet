using System;
using System.Linq.Expressions;
using Teronis.Tools;

namespace Teronis
{
    public static partial class ShortException
    {
        public static ArgumentNullException ArgumentNullException(Expression<Func<object?>> propertySelector, string? message = null)
        {
            var propertyName = ExpressionTools.GetReturnName(propertySelector);
            return new ArgumentNullException(propertyName, message);
        }
    }
}
