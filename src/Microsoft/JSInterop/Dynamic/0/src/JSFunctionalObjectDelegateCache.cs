using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop
{
    internal sealed class JSFunctionalObjectDelegateCache
    {
        public static JSFunctionalObjectDelegateCache Instance = new JSFunctionalObjectDelegateCache();

        private readonly static Type jsFunctionalObjectType = typeof(IJSFunctionalObject);

        public DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, object?[], object>> InvokeDelegateCache { get; }
        public DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, CancellationToken, object?[], object>> TokenCancellableInvokeDelegateCache { get; }
        public DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, TimeSpan, object?[], object>> TimeSpanCancellableInvokeDelegateCache { get; }

        private JSFunctionalObjectDelegateCache()
        {
            InvokeDelegateCache = new DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, object?[], object>>();
            TokenCancellableInvokeDelegateCache = new DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, CancellationToken, object?[], object>>();
            TimeSpanCancellableInvokeDelegateCache = new DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, TimeSpan, object?[], object>>();
        }

        private static DelegateType CreateDelegate<DelegateType>(
            MethodInfo methodInfo,
            ValueTaskType valueTaskType)
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

            /* We want a delegate equivalently to:
             * 
             * (jsFunctionalObject, identifier, [cancellationToken/timeSpan,] args) =>
             *      (object)jsFunctionalObject.methodName(identifier, [cancellationToken/timeSpan,] args);
             */

            var jsFunctionalObjectParameterExpression = Expression.Parameter(jsFunctionalObjectType, "jsFunctionalObject");

            var methodCall = Expression.Call(
                    instance: jsFunctionalObjectParameterExpression,
                    methodName: methodInfo.Name,
                    typeArguments: typeArguments,
                    arguments: parameters);

            var convertedMethodCall = Expression.Convert(methodCall, typeof(object));

            var lambdaParameters = new List<ParameterExpression>(parameters.Length + 1);
            lambdaParameters.Add(jsFunctionalObjectParameterExpression);
            lambdaParameters.AddRange(parameters);

            var lambda = Expression.Lambda<DelegateType>(convertedMethodCall, lambdaParameters);
            return lambda.Compile();
        }

        internal class DelegateCache<DelegateType>
            where DelegateType : Delegate
        {
            private Lazy<MethodInfo> genericValueTaskReturningDelegate = null!;
            private Lazy<MethodInfo> valueTaskReturningDelegate = null!;

            private Dictionary<Type, DelegateType> delegateByReturnTypeDictionary = null!;

            public DelegateCache()
            {
                /* We always assume Func<IJSFunctionalObject, .., object>. */

                var allParameterTypes = typeof(DelegateType).GetGenericArguments();

                var jsFunctionalObjectParameterExpressions = typeof(DelegateType)
                    .GetGenericArguments()
                    .Skip(1)
                    .SkipLast(1)
                    .ToArray();

                delegateByReturnTypeDictionary = new Dictionary<Type, DelegateType>();

                genericValueTaskReturningDelegate = new Lazy<MethodInfo>(() => {
                    return jsFunctionalObjectType.GetMethod(
                        nameof(JSObjectReferenceExtensions.InvokeAsync),
                        genericParameterCount: 1,
                        types: jsFunctionalObjectParameterExpressions)
                    ?? throw new InvalidOperationException();
                });

                valueTaskReturningDelegate = new Lazy<MethodInfo>(() => {
                    return jsFunctionalObjectType.GetMethod(
                        nameof(JSObjectReferenceExtensions.InvokeVoidAsync),
                        genericParameterCount: 0,
                        types: jsFunctionalObjectParameterExpressions)
                    ?? throw new InvalidOperationException();
                });
            }

            public DelegateType GetDelegate(ValueTaskType valueTaskReturnType)
            {
                if (delegateByReturnTypeDictionary.TryGetValue(valueTaskReturnType.Type, out var invocableFunction)) {
                    return invocableFunction;
                }

                var @delegate = valueTaskReturnType.HasGenericParameterType
                    ? CreateDelegate<DelegateType>(genericValueTaskReturningDelegate.Value, valueTaskReturnType)
                    : CreateDelegate<DelegateType>(valueTaskReturningDelegate.Value, valueTaskReturnType);

                delegateByReturnTypeDictionary.Add(valueTaskReturnType.Type, @delegate);
                return @delegate;
            }
        }
    }
}
