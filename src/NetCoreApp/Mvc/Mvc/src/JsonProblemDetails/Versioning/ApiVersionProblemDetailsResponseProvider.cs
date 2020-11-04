using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails.Versioning
{
    public class ApiVersionProblemDetailsResponseProvider : DefaultErrorResponseProvider
    {
        private readonly ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy;

        public ApiVersionProblemDetailsResponseProvider(ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy)
        {
            this.problemDetailsMiddlewareContextProxy = problemDetailsMiddlewareContextProxy
                ?? throw new System.ArgumentNullException(nameof(problemDetailsMiddlewareContextProxy));
        }

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var middleware = problemDetailsMiddlewareContextProxy.MiddlewareContext;
            middleware.MappableObject = context;
            var problemDetails = ProblemDetailsUtils.CreateMissingMapper(typeof(ApiVersionProblemDetailsMapper));
            middleware.FaultyConditionalResult = new ProblemDetailsResult(problemDetails);
            return new EmptyResult();
        }
    }
}
