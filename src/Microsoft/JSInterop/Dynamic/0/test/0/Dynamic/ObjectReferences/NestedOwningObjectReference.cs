using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop.Dynamic.ObjectReferences
{
    public class NestedOwningObjectReference : EmptyObjectReference
    {
        public List<EmptyObjectReference> ObjectReferences { get; }

        public NestedOwningObjectReference() =>
            ObjectReferences = new List<EmptyObjectReference>();

        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if (identifier == nameof(INestedOwningDynamicObject.GetNestedObjectAsync)) {
                var objectReference = new IdentifierPromisingObjectReference();
                ObjectReferences.Add(objectReference);
                return new ValueTask<TValue>((TValue)(object)objectReference);
            }

            return base.InvokeAsync<TValue>(identifier, args);
        }
    }
}
