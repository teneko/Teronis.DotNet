using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSLocalObjectReference : IJSObjectReference
    {
        ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(string objectName);
    }
}
