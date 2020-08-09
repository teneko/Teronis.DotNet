using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels.Filters
{
    public interface IActionModelFilter
    {
        bool IsAllowed(ActionModel? action);
    }
}
