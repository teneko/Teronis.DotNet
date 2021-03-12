using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IIdentifierPromisingDynamicObject : IJSDynamicObject
    {
        ValueTask<string> GetIdentifier();
    }
}
