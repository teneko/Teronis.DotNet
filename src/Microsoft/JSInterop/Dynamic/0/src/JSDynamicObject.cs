using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObject : DynamicObject, IJSDynamicObject
    {
        public IJSObjectReference JSObjectReference =>
            jsObjectReference;

        private readonly IJSObjectReference jsObjectReference;
        private readonly MethodDictionary methodDictionary;
        private readonly IJSFunctionalObject jsFunctionalObject;

        internal JSDynamicObject(IJSObjectReference jsObjectReference, MethodDictionary methodDictionary, IJSFunctionalObject jsFunctionalObject)
        {
            this.jsObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            this.methodDictionary = methodDictionary ?? throw new ArgumentNullException(nameof(methodDictionary));
            this.jsFunctionalObject = jsFunctionalObject;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result) => base.TryGetMember(binder, out result);

        private static string?[] GetPositionalArgumentNames(int numberOfArguments, IReadOnlyList<string> argumentNames)
        {
            var numberOfArgumentNames = argumentNames.Count;

            var positionalArgumentNames = new string?[numberOfArguments];
            var currentArgumentPosition = 0;

            for (; currentArgumentPosition < numberOfArguments - numberOfArgumentNames; currentArgumentPosition++) {
                positionalArgumentNames[currentArgumentPosition] = null;
            }

            foreach (var argumentName in argumentNames) {
                positionalArgumentNames[currentArgumentPosition] = argumentName;
                currentArgumentPosition++;
            }

            return positionalArgumentNames;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? arguments, out object? result)
        {
            arguments ??= new object[0];
            IReadOnlyList<Type>? genericParameterTypes;

            if (DynamicObjectUtils.TryGetInvokeMemberTypeArguments(binder, out var genericParameterTypeList)) {
                genericParameterTypes = genericParameterTypeList.ToList();
            } else {
                genericParameterTypes = null;
            }

            var positionalArgumentNames = GetPositionalArgumentNames(binder.CallInfo.ArgumentCount, binder.CallInfo.ArgumentNames);

            if (!methodDictionary.TryFindMethod(binder.Name, positionalArgumentNames, out var method)) {
                goto exit;
            }

            result = method.Invoke(jsFunctionalObject, jsObjectReference, genericParameterTypes, arguments);
            return true;

            exit:
            return base.TryInvokeMember(binder, arguments, out result);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] arguments) =>
           jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, arguments);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, cancellationToken, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, timeout, args);

        public ValueTask InvokeVoidAsync(string identifier, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, args);

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, cancellationToken, args);

        public ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, timeout, args);

        public ValueTask DisposeAsync() =>
            jsObjectReference.DisposeAsync();
    }
}
