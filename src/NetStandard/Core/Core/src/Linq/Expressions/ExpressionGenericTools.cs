// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public static string[] GetAnonymousTypeNames<OwnerType, ReturnType>(Expression<Func<OwnerType, ReturnType>> expression) =>
            ExpressionTools.GetAnonymousTypeNames(expression);

        public static string[] GetAnonymousTypeNames<ReturnType>(Expression<Func<ReturnType>> expression) =>
            ExpressionTools.GetAnonymousTypeNames(expression);
    }
}
