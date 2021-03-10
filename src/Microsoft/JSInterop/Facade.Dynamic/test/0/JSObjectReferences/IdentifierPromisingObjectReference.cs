using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSObjectReferences
{
    public class IdentifierPromisingObjectReference : JSObjectReferenceBase
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if (typeof(TValue) == typeof(string) && (args is null || args.Length == 0)) {
                return new ValueTask<TValue>((TValue)(object)identifier);
            }

            throw new NotSupportedException();
        }
    }
}
