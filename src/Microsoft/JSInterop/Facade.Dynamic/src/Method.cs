using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    internal class Method
    {
        public MethodInfo MethodInfo { get; }
        public ParameterList ParameterList { get; }
        public ValueTaskType ValueTaskType { get; }

        public string Name =>
            MethodInfo.Name;

        public Method(MethodInfo methodInfo, ParameterList parameterList, ValueTaskType valueTaskType)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ParameterList = parameterList ?? throw new ArgumentNullException(nameof(parameterList));
            ValueTaskType = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
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
        public object Invoke(IJSObjectReference jsObjectReference, params object?[] arguments)
        {
            var javascriptFunctionArguments = ParameterList.GetJavaScriptFunctionArguments(arguments);

            if (ParameterList.HasCancellationTokenParameter) {
                return IJSObjectReferenceUtils.GetInvocableWithCancellationTokenFunction(ValueTaskType)(
                    jsObjectReference,
                    GetJavaScriptFunctionIdentifier(),
                    ParameterList.GetCancellationToken(arguments),
                    javascriptFunctionArguments);
            }

            if (ParameterList.HasTimeSpanParameter) {
                return IJSObjectReferenceUtils.GetInvocableWithTimeSpanFunction(ValueTaskType)(
                    jsObjectReference,
                    GetJavaScriptFunctionIdentifier(),
                    ParameterList.GetTimeSpan(arguments),
                    javascriptFunctionArguments);
            }

            return IJSObjectReferenceUtils.GetInvocableFunction(ValueTaskType)(
                jsObjectReference,
                GetJavaScriptFunctionIdentifier(),
                javascriptFunctionArguments);
        }
    }
}
