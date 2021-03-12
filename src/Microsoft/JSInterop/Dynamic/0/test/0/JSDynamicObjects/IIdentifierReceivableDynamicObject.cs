using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IIdentifierReceivableDynamicObject : IJSDynamicObject
    {
        ValueTask<string> ReceiveIdentifier();
    }
}
