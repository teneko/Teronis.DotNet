using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssignments
{
    public interface IComponentPropertyAssignment
    {
        ValueTask<YetNullable<IAsyncDisposable>> TryAssignComponentProperty(
            IComponentPropertyInfo componentProperty,
            IJSFacadeResolver jsFacadeResolver);
    }
}
