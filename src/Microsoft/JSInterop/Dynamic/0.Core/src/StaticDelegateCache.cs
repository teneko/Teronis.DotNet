// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using Microsoft.JSInterop;
//using Teronis.Microsoft.JSInterop.Dynamic.Reflection;

//namespace Teronis.Microsoft.JSInterop
//{
//    internal sealed class StaticDelegateCache2
//    {
//        private static DelegateType CreateDelegate<DelegateType>(
//            Type instanceType,
//            MethodInfo methodInfo,
//            ValueTaskType valueTaskType,
//            Type convertingReturnType)
//            where DelegateType : Delegate
//        {
//            var parameters = methodInfo
//                .GetParameters()
//                .Select(x => Expression.Parameter(x.ParameterType, x.Name))
//                .ToArray();

//            Type[]? typeArguments;

//            if (valueTaskType.HasGenericParameterType) {
//                typeArguments = new[] { valueTaskType.GenericParameterType! };
//            } else {
//                typeArguments = null;
//            }

//            /* We want a delegate equivalently to:
//             * 
//             * (jsFunctionalObject, identifier, [cancellationToken/timeSpan,] args) =>
//             *      (object)jsFunctionalObject.methodName(identifier, [cancellationToken/timeSpan,] args);
//             */

//            var jsFunctionalObjectParameterExpression = Expression.Parameter(instanceType, "jsFunctionalObject");

//            var methodCall = Expression.Call(
//                    instance: jsFunctionalObjectParameterExpression,
//                    methodName: methodInfo.Name,
//                    typeArguments: typeArguments,
//                    arguments: parameters);

//            var convertedMethodCall = Expression.Convert(methodCall, convertingReturnType);

//            var lambdaParameters = new List<ParameterExpression>(parameters.Length + 1);
//            lambdaParameters.Add(jsFunctionalObjectParameterExpression);
//            lambdaParameters.AddRange(parameters);

//            var lambda = Expression.Lambda<DelegateType>(convertedMethodCall, lambdaParameters);
//            return lambda.Compile();
//        }

//        internal class DelegateCache<DelegateType>
//            where DelegateType : Delegate
//        {
//            public Type InstanceType { get; }

//            private Lazy<MethodInfo> genericValueTaskReturningDelegate = null!;
//            private Lazy<MethodInfo> valueTaskReturningDelegate = null!;

//            private Dictionary<Type, DelegateType> delegateByReturnTypeDictionary = null!;
//            private Type[] ParameterTypes = null!;

//            public DelegateCache(Type instanceType)
//            {
//                InstanceType = instanceType ?? throw new ArgumentNullException(nameof(instanceType));

//                /* We always assume Func<IJSFunctionalObject, .., object>. */

//                ParameterTypes = typeof(DelegateType).GetGenericArguments();

//                var jsFunctionalObjectParameterExpressions = ParameterTypes
//                    .Skip(1)
//                    .SkipLast(1)
//                    .ToArray();

//                delegateByReturnTypeDictionary = new Dictionary<Type, DelegateType>();

//                genericValueTaskReturningDelegate = new Lazy<MethodInfo>(() => {
//                    return 
//                });

//                valueTaskReturningDelegate = new Lazy<MethodInfo>(() => {
//                    return instanceType.GetMethod(
//                        nameof(JSObjectReferenceExtensions.InvokeVoidAsync),
//                        genericParameterCount: 0,
//                        types: jsFunctionalObjectParameterExpressions)
//                    ?? throw new InvalidOperationException();
//                });
//            }

//            public DelegateType GetDelegate(ValueTaskType valueTaskReturnType)
//            {
//                if (delegateByReturnTypeDictionary.TryGetValue(valueTaskReturnType.Type, out var invocableFunction)) {
//                    return invocableFunction;
//                }

//                var convertingReturnType = ParameterTypes[ParameterTypes.Length - 1];

//                MethodInfo methodInfo;

//                if (valueTaskReturnType.HasGenericParameterType) {
//                    var methodInfo = instanceType.GetMethod(
//                        nameof(JSObjectReferenceExtensions.InvokeAsync),
//                        genericParameterCount: 1,
//                        types: jsFunctionalObjectParameterExpressions)
//                    ?? throw new InvalidOperationException();
//                }

//                delegateByReturnTypeDictionary.Add(valueTaskReturnType.Type, @delegate);
//                return @delegate;
//            }
//        }
//    }
//}
