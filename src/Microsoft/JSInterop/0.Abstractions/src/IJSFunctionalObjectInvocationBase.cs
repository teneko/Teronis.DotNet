using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectInvocationBase
    {
        bool IsInterceptionStopped { get; }

        object?[] Arguments { get; }
        CancellationToken? CancellationToken { get; }
        string Identifier { get; }
        bool HasAlternativeResult { get; }
        bool HasAlternativeJSObjectReference { get; }
        IJSObjectReference JSObjectReferenceResulting { get; set; }
        IJSObjectReference JSObjectReferencePassed { get; }

        void StopInterception();
    }
}
