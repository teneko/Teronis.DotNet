using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails.Filters
{
    public class ProblemDetailsActionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ProblemDetailsOptions options;
        private readonly ProblemDetailsResultProvider problemDetailsResponseProvider;
        private readonly ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext;

        public ProblemDetailsActionFilter(ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext,
            ProblemDetailsResultProvider problemDetailsResponseProvider, IOptions<ProblemDetailsOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.problemDetailsResponseProvider = problemDetailsResponseProvider ?? throw new ArgumentNullException(nameof(problemDetailsResponseProvider));
            this.problemDetailsMiddlewareContext = problemDetailsMiddlewareContext ?? throw new ArgumentNullException(nameof(problemDetailsMiddlewareContext));
        }

        public int Order => options.ActionResultFilterOrder;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Skip consciously.
            return;
        }

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (problemDetailsMiddlewareContext.CanSkipFilter()) {
                return;
            }

            if (problemDetailsResponseProvider.TryCreateResult(actionExecutedContext, out var result)) {
                actionExecutedContext.Result = result;
                problemDetailsMiddlewareContext.Handled = true;
            }
        }
    }
}
