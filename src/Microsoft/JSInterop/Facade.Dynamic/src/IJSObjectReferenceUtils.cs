using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    internal static partial class IJSObjectReferenceUtils
    {
        static Type jsObjectReferenceExtensionsType = typeof(JSObjectReferenceExtensions);

        static IJSObjectReferenceUtils()
        {
            InitializePartInvocable();
            InitializePartInvocableWithCancellationToken();
            InitializePartInvocableWithTimeSpan();
        }

        //private static Func<IJSObjectReference, string, ThirdArgumentType, object?[], object> CreateThirdArgumentInvoker<ThirdArgumentType>()
        //{
        //    var function = (Func<IJSObjectReference, string, ThirdArgumentType, object?[], ValueTask>)Delegate.CreateDelegate(
        //        typeof(Func<IJSObjectReference, string, ThirdArgumentType, object?[], ValueTask>),
        //        valueTaskReturningInvokeFunction);

        //    Func<IJSObjectReference, string, ThirdArgumentType, object?[], object> weaklyTypedFunction = (jsObjectReference, thirdArgument, identifier, args) =>
        //        function(jsObjectReference, thirdArgument, identifier, args);

        //    return weaklyTypedFunction;
        //}

        private static DelegateType CreateInvoker<DelegateType>(MethodInfo methodInfo)
            where DelegateType : Delegate
        {
            var parameterExpressions = new List<ParameterExpression>(
                methodInfo.GetParameters().Select(x => Expression.Parameter(x.ParameterType)));

            var methodCallExpression = Expression.Convert(Expression.Call(genericValueTaskReturningInvokeFunction, parameterExpressions), typeof(object));
            return Expression.Lambda<DelegateType>(methodCallExpression, parameterExpressions).Compile();
        }
    }
}
