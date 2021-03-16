using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectInvocation : IJSFunctionalObjectInvocationBase<ValueTask>
    {
        internal abstract void Clone();
    }
}
