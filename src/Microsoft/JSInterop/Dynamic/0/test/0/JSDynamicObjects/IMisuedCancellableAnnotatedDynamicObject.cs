using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IMisuedCancellableAnnotatedDynamicObject : IJSObjectReferenceFacade
    {
        ValueTask ProvoceTooManyCancellableAnnotatedParameterException([Cancellable] CancellationToken cancellationToken, [Cancellable] TimeSpan timeout);
    }
}
