// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dynamitey;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Reflection;
using DynamiteyDynamic = Dynamitey.Dynamic;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    internal class Method
    {
        private static InvokeContext methodStaticContext;

        static Method() =>
            methodStaticContext = InvokeContext.CreateStatic(typeof(Method));

        private static async ValueTask<TValue> InterceptAndGetDeterminedValue<TValue>(
            IJSObjectInterceptor jsObjectInterceptor,
            IJSObjectReference jsObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes methodAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(
                jsObjectReference,
                identifier,
                cancellationToken,
                timeout,
                arguments,
                methodAttributes);

            await jsObjectInterceptor.InterceptInvokeAsync(invocation);
            return await invocation.GetDeterminedResult();
        }

        private static Dictionary<Type, Type> closedGenericValueTaskTypeByGenericArgumentType = new Dictionary<Type, Type>();
        private static Type openGenericValueTaskType = typeof(ValueTask<>);

        private static Type GetClosedGenericValueTaskType(Type genericArgumentType)
        {
            if (closedGenericValueTaskTypeByGenericArgumentType.TryGetValue(genericArgumentType, out var valueTaskType)) {
                return valueTaskType;
            }

            valueTaskType = openGenericValueTaskType.MakeGenericType(genericArgumentType);

            closedGenericValueTaskTypeByGenericArgumentType.Add(
                genericArgumentType,
                valueTaskType);

            return valueTaskType;
        }

        public MethodInfo MethodInfo { get; }
        public ParameterList ParameterList { get; }
        public ValueTaskType ResultingValueTaskType { get; }

        public string Name =>
            MethodInfo.Name;

        public Method(MethodInfo methodInfo, ParameterList parameterList, ValueTaskType resultingValueTaskType)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ParameterList = parameterList ?? throw new ArgumentNullException(nameof(parameterList));
            ResultingValueTaskType = resultingValueTaskType ?? throw new ArgumentNullException(nameof(resultingValueTaskType));
        }

        public string GetJavaScriptFunctionIdentifier()
        {
            var attribute = MethodInfo.GetCustomAttribute<IdentifierAttribute>();

            if (!(attribute is null)) {
                return attribute.Identifier;
            }

            return MethodInfo.Name;
        }

        public object GetReturnValue(
            IJSObjectInterceptor jsObjectInterceptor,
            IJSObjectReference jsObjectReference,
            IReadOnlyList<Type>? genericTypeArguments,
            object?[] arguments,
            ICustomAttributes memberInfoAttributes)
        {
            Type? alternativeValueTaskGenericTypeParameter;

            if (!(genericTypeArguments is null)) {
                if (genericTypeArguments.Count > 1) {
                    throw new NotSupportedException("Only one generic type argument is allowed.");
                }

                alternativeValueTaskGenericTypeParameter = genericTypeArguments?.SingleOrDefault();
            } else {
                alternativeValueTaskGenericTypeParameter = null;
            }

            ValueTaskType valueTaskReturnType;

            if (alternativeValueTaskGenericTypeParameter != null) {
                var genericValueTask = GetClosedGenericValueTaskType(alternativeValueTaskGenericTypeParameter);
                valueTaskReturnType = new ValueTaskType(genericValueTask, alternativeValueTaskGenericTypeParameter);
            } else {
                valueTaskReturnType = ResultingValueTaskType;
            }

            var javaScriptFunctionArguments = ParameterList.GetJavaScriptFunctionArguments(arguments);
            var javaScriptFunctionIdentifier = GetJavaScriptFunctionIdentifier();

            CancellationToken? javaScriptCancellationToken = ParameterList.HasCancellationTokenParameter
                ? (CancellationToken?)ParameterList.GetCancellationToken(arguments)
                : null;

            TimeSpan? javaScriptTimeout = ParameterList.HasTimeoutParameter
                ? (TimeSpan?)ParameterList.GetTimeSpan(arguments)
                : null;

            if (ResultingValueTaskType.HasGenericParameterType) {
                return DynamiteyDynamic.InvokeMember(
                    methodStaticContext,
                    new InvokeMemberName(nameof(InterceptAndGetDeterminedValue), new Type[] { valueTaskReturnType.GenericParameterType! }),
                    args: new object?[] {
                        jsObjectInterceptor,
                        jsObjectReference,
                        javaScriptFunctionIdentifier,
                        javaScriptCancellationToken,
                        javaScriptTimeout,
                        javaScriptFunctionArguments,
                        memberInfoAttributes
                    });
            } else {
                var jsObjectInvocation = new JSObjectInvocation(
                    jsObjectReference,
                    javaScriptFunctionIdentifier,
                    javaScriptCancellationToken,
                    javaScriptTimeout,
                    javaScriptFunctionArguments,
                    memberInfoAttributes);

                return new ValueTask(new Func<Task>(async () => {
                    await jsObjectInterceptor.InterceptInvokeVoidAsync(jsObjectInvocation);
                    await jsObjectInvocation.GetDeterminedResult();
                })());
            }
        }
    }
}
