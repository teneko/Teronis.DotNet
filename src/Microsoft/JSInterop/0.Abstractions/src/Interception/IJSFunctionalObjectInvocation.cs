using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocation : IJSFunctionalObjectInvocationBase<ValueTask>
    {
        internal abstract void Clone();
    }
}
