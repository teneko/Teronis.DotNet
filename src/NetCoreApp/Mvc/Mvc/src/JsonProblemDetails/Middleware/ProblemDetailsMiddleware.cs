using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public class ProblemDetailsMiddleware
    {
        static bool tryGetFaultyConditionalResult(ProblemDetailsMiddlewareContext middlewareContext, [MaybeNullWhen(false)] out ProblemDetailsResult result)
        {
            if (!(middlewareContext.MappableObject is null)) {
                if (!(middlewareContext.FaultyConditionalResult is null)) {
                    result = middlewareContext.FaultyConditionalResult;
                } else {
                    var mappableObjectType = middlewareContext.MappableObject.GetType();
                    var problemTitle = $"You specified a problem details provider but your forgot to add the belonging mapper for objects of type {mappableObjectType.FullName}.";
                    var problemDetails = ProblemDetailsUtils.CreateDefault(problemTitle);
                    result = new ProblemDetailsResult(problemDetails);
                }

                return true;
            }

            result = null;
            return false;
        }

        static async Task<bool> tryHandleResponse(HttpContext httpContext, object? mappableObject, ProblemDetailsMiddlewareContext middlewareContext,
               ProblemDetailsResponseProvider problemDetailsResponseProvider, IActionResultExecutor<ProblemDetailsResult> resultExecutor)
        {
            if (middlewareContext.Handled) {
                goto exit;
            }

            var lazyActionContext = new Lazy<ActionContext>(() => {
                var routeData = httpContext.GetRouteData() ?? new RouteData();
                var emptyActionDecriptor = new ActionDescriptor();
                var actionContext = new ActionContext(httpContext, routeData, emptyActionDecriptor);
                return actionContext;
            });

            if (problemDetailsResponseProvider.TryCreateResponse(httpContext, mappableObject, out var result) 
                || tryGetFaultyConditionalResult(middlewareContext, out result)) {
                problemDetailsResponseProvider.PrepareHttpResponse(httpContext.Response, result);
                await resultExecutor.ExecuteAsync(lazyActionContext.Value, result);
                middlewareContext.Handled = true;
                return true;
            } 

            exit:
            return false;
        }

        private readonly RequestDelegate nextRequestDelegate;
        private readonly ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy;
        private readonly ILogger? logger;
        private readonly ProblemDetailsResponseProvider problemDetailsResponseProvider;

        public ProblemDetailsMiddleware(RequestDelegate nextRequestDelegate, ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy,
            ProblemDetailsResponseProvider problemDetailsResponseProvider, ILogger<ProblemDetailsMiddleware>? logger)
        {
            this.nextRequestDelegate = nextRequestDelegate ?? throw new ArgumentNullException(nameof(nextRequestDelegate));
            this.problemDetailsMiddlewareContextProxy = problemDetailsMiddlewareContextProxy ?? throw new ArgumentNullException(nameof(problemDetailsMiddlewareContextProxy));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.problemDetailsResponseProvider = problemDetailsResponseProvider ?? throw new ArgumentNullException(nameof(problemDetailsResponseProvider));
        }

        protected virtual Task StartResponseAsync(HttpContext context) =>
            Task.CompletedTask;

        public async Task Invoke(HttpContext httpContext, ProblemDetailsMiddlewareContext middlewareContext, IActionResultExecutor<ProblemDetailsResult> resultExecutor)
        {
            var response = httpContext.Response;
            middlewareContext = middlewareContext ?? throw new ArgumentNullException(nameof(middlewareContext));
            problemDetailsMiddlewareContextProxy.MiddlewareContext = middlewareContext;

            static void logResponseAlreadyStarted(ILogger? logger, Exception? error)
            {
                if (logger is null) {
                    return;
                }

                var errorMessage = "The response has been already started.";

                if (error is null) {
                    logger.LogError(errorMessage, errorMessage);
                } else {
                    logger.LogError(errorMessage);
                }
            }

            try {
                await nextRequestDelegate(httpContext);

                if (response.HasStarted) {
                    logResponseAlreadyStarted(logger, null);
                    return;
                }

                object? mappableObject = middlewareContext.MappableObject;
                await tryHandleResponse(httpContext, mappableObject, middlewareContext, problemDetailsResponseProvider, resultExecutor);
            } catch (Exception error) {
                if (response.HasStarted) {
                    logResponseAlreadyStarted(logger, error);
                    throw;
                }

                if (!await tryHandleResponse(httpContext, error, middlewareContext, problemDetailsResponseProvider, resultExecutor)) {
                    throw;
                }
            }
        }
    }
}
