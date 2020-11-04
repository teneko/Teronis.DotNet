using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails.Filters
{
    public class ProblemDetailsExceptionFilter : IExceptionFilter, IOrderedFilter
    {
        private readonly ProblemDetailsOptions options;
        private readonly ProblemDetailsResponseProvider problemDetailsResponseProvider;
        private readonly ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext;

        public ProblemDetailsExceptionFilter(ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext,
            ProblemDetailsResponseProvider problemDetailsResponseProvider, IOptions<ProblemDetailsOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.problemDetailsResponseProvider = problemDetailsResponseProvider ?? throw new ArgumentNullException(nameof(problemDetailsResponseProvider));
            this.problemDetailsMiddlewareContext = problemDetailsMiddlewareContext ?? throw new ArgumentNullException(nameof(problemDetailsMiddlewareContext));
        }

        public int Order => options.ExceptionFilterOrder;

        public void OnException(ExceptionContext exceptionContext)
        {
            if (problemDetailsMiddlewareContext.CanSkipFilter()) {
                return;
            }

            if (problemDetailsResponseProvider.TryCreateResponse(exceptionContext, out var result)) {
                problemDetailsResponseProvider.PrepareHttpResponse(exceptionContext.HttpContext.Response, result);
                exceptionContext.Result = result;
                exceptionContext.ExceptionHandled = true;
                problemDetailsMiddlewareContext.Handled = true;
            }
        }
    }
}
