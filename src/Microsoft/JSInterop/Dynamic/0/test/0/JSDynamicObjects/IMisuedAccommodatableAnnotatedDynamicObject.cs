using System.Collections;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IMisuedAccommodatableAnnotatedDynamicObject : IJSDynamicObject
    {
        ValueTask ProvoceBadParameterAfterAccommodatableAnnotatedParameterException([Accommodatable] IEnumerable arguments, object ballast);
    }
}
