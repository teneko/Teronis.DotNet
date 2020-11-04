using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.JsonProblemDetails
{
    public interface IJsonProblemDetailsBuilder
    {
        IServiceCollection Services { get; }
    }
}