using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions.Utils
{
    public class DelegateExpression
    {
        public static Expression<Action> Expr<T>(Expression<Action> action) =>
            action;

        public static Expression<Action<T>> Expr<T>(Expression<Action<T>> action) =>
            action;

        public static Expression<Action<T1, T2>> Expr<T1, T2>(Expression<Action<T1, T2>> action) =>
            action;

        public static Expression<Action<T1, T2, T3>> Expr<T1, T2, T3>(Expression<Action<T1, T2, T3>> action) =>
            action;

        public static Expression<Action<T1, T2, T3, T4>> Expr<T1, T2, T3, T4>(Expression<Action<T1, T2, T3, T4>> action) =>
            action;

        public static Expression<Action<T1, T2, T3, T4, T5>> Expr<T1, T2, T3, T4, T5>(Expression<Action<T1, T2, T3, T4, T5>> action) =>
            action;

        public static Expression<Action<T1, T2, T3, T4, T5, T6>> Expr<T1, T2, T3, T4, T5, T6>(Expression<Action<T1, T2, T3, T4, T5, T6>> action) =>
            action;

        public static Expression<Action<T1, T2, T3, T4, T5, T6, T7>> Expr<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T1, T2, T3, T4, T5, T6, T7>> action) =>
            action;

        public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> Expr<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> action) =>
            action;

        public static Expression<Func<T>> Expr<T>(Expression<Func<T>> action) =>
            action;

        public static Expression<Func<T1, T2>> Expr<T1, T2>(Expression<Func<T1, T2>> action) =>
            action;

        public static Expression<Func<T1, T2, T3>> Expr<T1, T2, T3>(Expression<Func<T1, T2, T3>> action) =>
            action;

        public static Expression<Func<T1, T2, T3, T4>> Expr<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4>> action) =>
            action;

        public static Expression<Func<T1, T2, T3, T4, T5>> Expr<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5>> action) =>
            action;

        public static Expression<Func<T1, T2, T3, T4, T5, T6>> Expr<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6>> action) =>
            action;

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7>> Expr<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7>> action) =>
            action;

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> Expr<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> action) =>
            action;
    }
}
