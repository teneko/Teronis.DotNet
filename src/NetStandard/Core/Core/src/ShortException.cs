using System;
using System.Linq.Expressions;
using Teronis.Utils;

namespace Teronis
{
    public static partial class ShortException
    {
        public static ArgumentNullException ArgumentNullException(Expression<Func<object?>> propertySelector, string? message = null)
        {
            var propertyName = ExpressionUtils.GetReturnName(propertySelector);
            return new ArgumentNullException(propertyName, message);
        }
    }
}
