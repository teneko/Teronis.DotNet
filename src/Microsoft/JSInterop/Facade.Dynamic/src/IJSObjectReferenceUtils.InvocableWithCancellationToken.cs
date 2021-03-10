using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    partial class IJSObjectReferenceUtils
    {
        private static MethodInfo genericValueTaskReturningInvokeWithCancellationTokenFunctionType = null!;
        private static MethodInfo valueTaskReturningInvokeWithCancellationTokenFunctionType = null!;

        private static Dictionary<Type, Func<IJSObjectReference, string, CancellationToken, object?[], object>> invocableWithCancellationTokenFunctions = null!;

        private static void InitializePartInvocableWithCancellationToken()
        {
            invocableWithCancellationTokenFunctions = new Dictionary<Type, Func<IJSObjectReference, string, CancellationToken, object?[], object>>();

            genericValueTaskReturningInvokeWithCancellationTokenFunctionType =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeAsync),
                    genericParameterCount: 1,
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(CancellationToken),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();

            valueTaskReturningInvokeWithCancellationTokenFunctionType =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeVoidAsync),
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(CancellationToken),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();
        }

        public static Func<IJSObjectReference, string, CancellationToken, object?[], object> GetInvocableWithCancellationTokenFunction(ValueTaskType valueTaskType)
        {
            if (invocableWithCancellationTokenFunctions.TryGetValue(valueTaskType.Type, out var invocableFunction)) {
                return invocableFunction;
            }

            var invokeFunction = valueTaskType.HasGenericParameterType
                ? CreateInvoker<Func<IJSObjectReference, string, CancellationToken, object?[], object>>(genericValueTaskReturningInvokeWithCancellationTokenFunctionType, valueTaskType)
                : CreateInvoker<Func<IJSObjectReference, string, CancellationToken, object?[], object>>(valueTaskReturningInvokeWithCancellationTokenFunctionType, valueTaskType);

            invocableWithCancellationTokenFunctions.Add(valueTaskType.Type, invokeFunction);
            return invokeFunction;
        }
    }
}
