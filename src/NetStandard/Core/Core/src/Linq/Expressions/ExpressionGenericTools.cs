using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class ExpressionGenericTools
    {
        public static Type GetReturnType<OwnerType, ReturnType>(Expression<Func<OwnerType, ReturnType>> expression) =>
            ExpressionTools.GetReturnType(expression);

        public static Type GetReturnType<ReturnType>(Expression<Func<ReturnType>> expression) =>
            ExpressionTools.GetReturnType(expression);

        public static string GetReturnName<OwnerType, ReturnType>(Expression<Func<OwnerType,  ReturnType>> expression) =>
            ExpressionTools.GetReturnName(expression);

        public static string GetReturnName<ReturnType>(Expression<Func<ReturnType>> expression) =>
            ExpressionTools.GetReturnName(expression);

        public static string[] GetAnonTypeNames<OwnerType, ReturnType>(Expression<Func<OwnerType, ReturnType>> expression) =>
            ExpressionTools.GetAnonTypeNames(expression);

        public static string[] GetAnonTypeNames<ReturnType>(Expression<Func<ReturnType>> expression) =>
            ExpressionTools.GetAnonTypeNames(expression);
    }
}
