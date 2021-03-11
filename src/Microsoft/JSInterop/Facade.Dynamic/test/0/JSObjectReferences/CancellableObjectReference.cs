using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSObjectReferences
{
    public class CancellableObjectReference : JSEmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            if (cancellationToken.IsCancellationRequested) {
                throw new ObjectReferenceInvocationCanceledException(identifier, args);
            }

            return base.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }

    }
}
