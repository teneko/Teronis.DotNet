using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects
{
    public interface IMomentDynamicObject : IJSDynamicObject
    {
        ValueTask<IJSObjectReference> moment(string date);
    }
}
