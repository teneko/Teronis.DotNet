using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public class JSObjectProxy : DynamicObject, IJSObjectReferenceFacade
    {
        public IJSObjectReference JSObjectReference => jsObjectReference;

        private readonly IJSObjectReference jsObjectReference;
        private readonly MethodDictionary methodDictionary;

        internal JSObjectProxy(IJSObjectReference jsObjectReference, MethodDictionary methodDictionary)
        {
            this.jsObjectReference = jsObjectReference;
            this.methodDictionary = methodDictionary ?? throw new System.ArgumentNullException(nameof(methodDictionary));
        }

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

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            if (args is null) {
                result = null;
                return false;
            }

            var positionalArgumentNames = GetPositionalArgumentNames(binder.CallInfo.ArgumentCount, binder.CallInfo.ArgumentNames);

            if (!methodDictionary.TryFindMethod(binder.Name, positionalArgumentNames, out var method)) {
                result = null;
                return false;
            }

            result = method.Invoke(jsObjectReference, args);
            return true;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            jsObjectReference.InvokeAsync<TValue>(identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            jsObjectReference.InvokeAsync<TValue>(identifier, cancellationToken, args);

        public ValueTask DisposeAsync() =>
            JSObjectReference.DisposeAsync();
    }
}
