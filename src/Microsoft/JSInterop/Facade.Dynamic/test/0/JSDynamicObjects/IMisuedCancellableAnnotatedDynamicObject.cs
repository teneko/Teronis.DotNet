using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSDynamicObjects
{
    public interface IMisuedCancellableAnnotatedDynamicObject : IJSDynamicObject
    {
        ValueTask ProvoceTooManyCancellableAnnotatedParameterException([Cancellable] CancellationToken cancellationToken, [Cancellable] TimeSpan timeout);
    }
}
