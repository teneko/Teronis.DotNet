using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels.Filters
{
    public interface IApplicationModelFilter
    {
        bool IsAllowed(ApplicationModel appliaction);
    }
}
