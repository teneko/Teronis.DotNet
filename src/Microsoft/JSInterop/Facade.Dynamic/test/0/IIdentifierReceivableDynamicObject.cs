using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public interface IIdentifierReceivableDynamicObject : IJSDynamicObject
    {
        ValueTask<string> ReceiveIdentifier();
    }
}
