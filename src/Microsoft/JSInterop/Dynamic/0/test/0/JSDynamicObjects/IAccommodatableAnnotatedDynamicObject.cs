using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IAccommodatableAnnotatedDynamicObject : IJSDynamicObject
    {
        ValueTask<object?[]> GetJavaScriptArguments(object? ballast, [Cancellable] CancellationToken cancellationToken, [Accommodatable] params object?[] jsArguments);
    }
}
