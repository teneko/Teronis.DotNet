using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels.Filters
{
    public interface IParameterModelFilter
    {
        bool IsAllowed(ParameterModel? parameter);
    }
}
