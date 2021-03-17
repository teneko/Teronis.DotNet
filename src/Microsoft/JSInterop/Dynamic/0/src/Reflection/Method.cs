using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal class Method
    {
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
        public ValueTaskType ValueTaskReturnType { get; }

        public string Name =>
            MethodInfo.Name;

        public Method(MethodInfo methodInfo, ParameterList parameterList, ValueTaskType valueTaskType)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ParameterList = parameterList ?? throw new ArgumentNullException(nameof(parameterList));
            ValueTaskReturnType = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
        }

        public string GetJavaScriptFunctionIdentifier()
        {
            var attribute = MethodInfo.GetCustomAttribute<IdentifierAttribute>();

            if (!(attribute is null)) {
                return attribute.Identifier;
            }

            return MethodInfo.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectReference"></param>
        /// <returns><see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/> with a generic parameter of type <see cref="ValueTaskType.GenericParameterType"/>.</returns>
        public object Invoke(
            IJSFunctionalObject jsFunctionalObject,
            IJSObjectReference jsObjectReference,
            IReadOnlyList<Type>? genericTypeArguments,
            object?[] arguments)
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
                valueTaskReturnType = ValueTaskReturnType;
            }

            var javaScriptFunctionArguments = ParameterList.GetJavaScriptFunctionArguments(arguments);
            var javaScriptFunctionIdentifier = GetJavaScriptFunctionIdentifier();

            if (ParameterList.HasCancellationTokenParameter) {
                return JSFunctionalObjectDelegateCache.Instance.TokenCancellableInvokeDelegateCache.GetDelegate(valueTaskReturnType)(
                    jsFunctionalObject, jsObjectReference, javaScriptFunctionIdentifier, ParameterList.GetCancellationToken(arguments), javaScriptFunctionArguments);
            }

            if (ParameterList.HasTimeSpanParameter) {
                return JSFunctionalObjectDelegateCache.Instance.TimeSpanCancellableInvokeDelegateCache.GetDelegate(valueTaskReturnType)(
                    jsFunctionalObject, jsObjectReference, javaScriptFunctionIdentifier, ParameterList.GetTimeSpan(arguments), javaScriptFunctionArguments);
            }

            return JSFunctionalObjectDelegateCache.Instance.InvokeDelegateCache.GetDelegate(valueTaskReturnType)(
                jsFunctionalObject, jsObjectReference, javaScriptFunctionIdentifier, javaScriptFunctionArguments);
        }
    }
}
