using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public class JSDynamicObject : DynamicObject, IJSDynamicObject
    {
        public IJSObjectReference JSObjectReference =>
            jsObjectReference;

        private readonly IJSObjectReference jsObjectReference;
        private readonly MethodDictionary methodDictionary;

        internal JSDynamicObject(IJSObjectReference jsObjectReference, MethodDictionary methodDictionary)
        {
            this.jsObjectReference = jsObjectReference;
            this.methodDictionary = methodDictionary ?? throw new System.ArgumentNullException(nameof(methodDictionary));
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

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            if (args is null) {
                goto exit;
            }

            var positionalArgumentNames = GetPositionalArgumentNames(binder.CallInfo.ArgumentCount, binder.CallInfo.ArgumentNames);

            if (!methodDictionary.TryFindMethod(binder.Name, positionalArgumentNames, out var method)) {
                goto exit;
            }

            result = method.Invoke(jsObjectReference, args);
            return true;

            exit:
            return base.TryInvokeMember(binder, args, out result);
        }

        public ValueTask DisposeAsync() =>
            JSObjectReference.DisposeAsync();
    }
}
