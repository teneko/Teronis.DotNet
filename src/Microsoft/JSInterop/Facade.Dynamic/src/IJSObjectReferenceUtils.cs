using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        private static DelegateType CreateInvoker<DelegateType>(MethodInfo methodInfo, ValueTaskType valueTaskType)
            where DelegateType : Delegate
        {
            var parameters = methodInfo
                .GetParameters()
                .Select(x => Expression.Parameter(x.ParameterType, x.Name))
                .ToArray();

            Type[]? typeArguments;

            if (valueTaskType.HasGenericParameterType) {
                typeArguments = new[] { valueTaskType.GenericParameterType! };
            } else {
                typeArguments = null;
            }

            var methodCall = Expression.Call(
                    type: methodInfo.DeclaringType!,
                    methodName: methodInfo.Name,
                    typeArguments: typeArguments,
                    arguments: parameters);

            var convertedMethodCall = Expression.Convert(methodCall, typeof(object));
            var lambda = Expression.Lambda<DelegateType>(convertedMethodCall, parameters);
            return lambda.Compile();
        }
    }
}
