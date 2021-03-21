using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IExplicitGenericConstrainedDynamicObject : IJSObjectReferenceFacade
    {
        ValueTask<T> TakeAndReturnBallast<T>(T ballast);
    }
}
