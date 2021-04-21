// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails.Filters
{
    public class ProblemDetailsExceptionFilter : IExceptionFilter, IOrderedFilter
    {
        private readonly ProblemDetailsOptions options;
        private readonly ProblemDetailsResultProvider problemDetailsResponseProvider;
        private readonly ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext;

        public ProblemDetailsExceptionFilter(
            ProblemDetailsMiddlewareContext problemDetailsMiddlewareContext,
            ProblemDetailsResultProvider problemDetailsResponseProvider,
            IOptions<ProblemDetailsOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.problemDetailsResponseProvider = problemDetailsResponseProvider ?? throw new ArgumentNullException(nameof(problemDetailsResponseProvider));
            this.problemDetailsMiddlewareContext = problemDetailsMiddlewareContext ?? throw new ArgumentNullException(nameof(problemDetailsMiddlewareContext));
        }

        public int Order => options.ExceptionFilterOrder;

        public void OnException(ExceptionContext exceptionContext)
        {
            options.LogExceptionFromExceptionFilter?.Invoke(exceptionContext.Exception);

            if (problemDetailsMiddlewareContext.IsFilterSkippable()) {
                return;
            }

            if (problemDetailsResponseProvider.TryCreateResult(exceptionContext, out var result)) {
                exceptionContext.Result = result;
                exceptionContext.ExceptionHandled = true;
                problemDetailsMiddlewareContext.SetHandled(result);
            }
        }
    }
}
