using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSObjectReferences
{
    public class JSArgumentsPromisingObjectReference : JSEmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            new ValueTask<TValue>((TValue)(object)args!);

        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            new ValueTask<TValue>((TValue)(object)args!);
    }
}
