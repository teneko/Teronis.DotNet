using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    public interface IProblemDetailsMapper
    {
        bool CanMap();
        ProblemDetails CreateProblemDetails();
    }
}
