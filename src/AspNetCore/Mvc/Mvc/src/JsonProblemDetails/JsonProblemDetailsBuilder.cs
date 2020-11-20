using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.JsonProblemDetails
{
    internal class JsonProblemDetailsBuilder : IJsonProblemDetailsBuilder
    {
        public IServiceCollection Services { get; }

        public JsonProblemDetailsBuilder(IServiceCollection services) =>
            Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}
