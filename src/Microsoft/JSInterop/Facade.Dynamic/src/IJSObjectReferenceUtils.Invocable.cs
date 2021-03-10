using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    partial class IJSObjectReferenceUtils
    {
        private static MethodInfo genericValueTaskReturningInvokeFunction = null!;
        private static MethodInfo valueTaskReturningInvokeFunction = null!;

        private static Dictionary<Type, Func<IJSObjectReference, string, object?[], object>> invocableFunctions = null!;

        private static void InitializePartInvocable() {
            invocableFunctions = new Dictionary<Type, Func<IJSObjectReference, string, object?[], object>>();

            genericValueTaskReturningInvokeFunction =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeAsync),
                    genericParameterCount: 1,
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();

            valueTaskReturningInvokeFunction =
                jsObjectReferenceExtensionsType.GetMethod(
                    nameof(JSObjectReferenceExtensions.InvokeVoidAsync),
                    genericParameterCount: 0,
                    types: new[] {
                        typeof(IJSObjectReference),
                        typeof(string),
                        typeof(object?[])
                    })
                ?? throw new InvalidOperationException();
        }

        public static Func<IJSObjectReference, string, object?[], object> GetInvocableFunction(ValueTaskType valueTaskType)
        {
            if (invocableFunctions.TryGetValue(valueTaskType.Type, out var invocableFunction)) {
                return invocableFunction;
            }

            var invokeFunction = valueTaskType.HasGenericParameterType
                ? CreateInvoker<Func<IJSObjectReference, string, object?[], object>>(genericValueTaskReturningInvokeFunction, valueTaskType)
                : CreateInvoker<Func<IJSObjectReference, string, object?[], object>>(valueTaskReturningInvokeFunction, valueTaskType);

            invocableFunctions.Add(valueTaskType.Type, invokeFunction);
            return invokeFunction;
        }
    }
}
