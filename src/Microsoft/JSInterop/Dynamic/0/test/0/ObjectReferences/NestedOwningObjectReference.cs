using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop.Dynamic.ObjectReferences
{
    public class NestedOwningObjectReference : EmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if (identifier == nameof(INestedOwningDynamicObject.GetNestedObjectAsync)) {
                return new ValueTask<TValue>((TValue)(object)new IdentifierPromisingObjectReference());
            }

            return base.InvokeAsync<TValue>(identifier, args);
        }
    }
}
