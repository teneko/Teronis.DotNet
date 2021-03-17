using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects
{
    public interface IMomentDynamicObject : IJSModule
    {
        ValueTask<IJSObjectReference> moment(string date);
    }
}
