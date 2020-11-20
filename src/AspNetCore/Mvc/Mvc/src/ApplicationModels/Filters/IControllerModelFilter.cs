using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels.Filters
{
    public interface IControllerModelFilter
    {
        bool IsAllowed(ControllerModel? controller);
    }
}
