using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSDynamicObjects
{
    public interface ICancellableAnnotatedDynamicObject : IJSDynamicObject
    {
        ValueTask CancelViaCancellationToken(string cancellationReadon, [Cancellable] CancellationToken cancellationToken, object? ballast);
        ValueTask CancelViaTimeSpan(string cancellationReadon, [Cancellable] TimeSpan timeout, object? ballast);
    }
}
