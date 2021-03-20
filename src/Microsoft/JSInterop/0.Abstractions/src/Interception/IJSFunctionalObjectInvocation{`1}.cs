using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocation<TValue> : IJSFunctionalObjectInvocationBase<ValueTask<TValue>>
    {
        Type GenericTaskArgumentType { get; }

        internal abstract void Clone();
    }
}
