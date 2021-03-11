using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSDynamicObjects
{
    public interface INotOfTypeValueTaskDynamicObject : IJSDynamicObject
    {
        string ProvoceNotOfTypeValueTaskException();
    }
}
