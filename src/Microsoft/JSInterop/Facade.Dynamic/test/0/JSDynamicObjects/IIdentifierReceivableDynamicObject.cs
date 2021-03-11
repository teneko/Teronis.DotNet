using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSDynamicObjects
{
    public interface IIdentifierReceivableDynamicObject : IJSDynamicObject
    {
        ValueTask<string> ReceiveIdentifier();
    }
}
