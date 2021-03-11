using System.Collections;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.JSDynamicObjects
{
    public interface IMisuedAccommodatableAnnotatedDynamicObject : IJSDynamicObject
    {
        ValueTask ProvoceBadParameterAfterAccommodatableAnnotatedParameterException([Accommodatable] IEnumerable arguments, object ballast);
    }
}
