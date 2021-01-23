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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="mappableObject"></param>
        /// <param name="middlewareContext"></param>
        /// <param name="problemDetailsResponseProvider"></param>
        /// <param name="resultExecutor"></param>
        /// <param name="logger"></param>
        /// <returns>
        /// Value <see cref="true"/> if map result could be created. 
        /// Value <see cref="false"/> if mapper couldn't be found. 
        /// Value <see cref="null"/> if response has been started.
        /// </returns>
        static async Task<bool?> tryStartResponse(HttpContext httpContext, object? mappableObject, ProblemDetailsMiddlewareContext middlewareContext,
               ProblemDetailsResultProvider problemDetailsResponseProvider, IActionResultExecutor<ProblemDetailsResult> resultExecutor, ILogger? logger)
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

            if (problemDetailsResponseProvider.TryCreateResult(httpContext, mappableObject, out var result)
                || tryGetFaultyConditionalResult(middlewareContext, out result)) {
                if (httpContext.Response.HasStarted) {
                    return null;
                }

                await resultExecutor.ExecuteAsync(lazyActionContext.Value, result);
                middlewareContext.SetHandled(result);
                return true;
            }

            exit:
            return false;
        }

        private readonly RequestDelegate nextRequestDelegate;
        private readonly ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy;
        private readonly ILogger? logger;
        private readonly ProblemDetailsResultProvider problemDetailsResponseProvider;

        public ProblemDetailsMiddleware(RequestDelegate nextRequestDelegate, ProblemDetailsMiddlewareContextProxy problemDetailsMiddlewareContextProxy,
            ProblemDetailsResultProvider problemDetailsResponseProvider, ILogger<ProblemDetailsMiddleware>? logger)
        {
            this.nextRequestDelegate = nextRequestDelegate ?? throw new ArgumentNullException(nameof(nextRequestDelegate));

            this.problemDetailsMiddlewareContextProxy = problemDetailsMiddlewareContextProxy
                ?? throw new ArgumentNullException(nameof(problemDetailsMiddlewareContextProxy));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.problemDetailsResponseProvider = problemDetailsResponseProvider
                ?? throw new ArgumentNullException(nameof(problemDetailsResponseProvider));
        }

        protected virtual Task StartResponseAsync(HttpContext context) =>
            Task.CompletedTask;

        public async Task Invoke(HttpContext httpContext, ProblemDetailsMiddlewareContext middlewareContext,
            IActionResultExecutor<ProblemDetailsResult> resultExecutor)
        {
            var response = httpContext.Response;
            middlewareContext = middlewareContext ?? throw new ArgumentNullException(nameof(middlewareContext));
            problemDetailsMiddlewareContextProxy.MiddlewareContext = middlewareContext;

            try {
                await nextRequestDelegate(httpContext);
                object? mappableObject = middlewareContext.MappableObject;

                if (response.HasStarted && !(mappableObject is null)) {
                    logResponseAlreadyStarted(logger, null);
                } else if (response.HasStarted) {
                    return;
                }

                var responseHasStarted = response.HasStarted;
                var isMappableObjectNull = mappableObject is null;

                // The result does not matter us.
                await tryStartResponse(httpContext, mappableObject, middlewareContext, problemDetailsResponseProvider, resultExecutor, logger);
            } catch (Exception error) {
                var result = await tryStartResponse(httpContext, error, middlewareContext, problemDetailsResponseProvider, resultExecutor, logger);

                if (result == null) {
                    logResponseAlreadyStarted(logger, error);
                } else if (!result.Value) {
                    // If the exception is not being able to be mapped we rethrow.
                    throw;
                }
            }
        }
    }
}
