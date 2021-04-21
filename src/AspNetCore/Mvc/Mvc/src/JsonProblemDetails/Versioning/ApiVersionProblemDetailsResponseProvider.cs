// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails.Versioning
{
    public class ApiVersionProblemDetailsResponseProvider : DefaultErrorResponseProvider
    {
        private readonly ProblemDetailsMiddlewareContextProvider problemDetailsMiddlewareContextProvider;

        public ApiVersionProblemDetailsResponseProvider(ProblemDetailsMiddlewareContextProvider problemDetailsMiddlewareContextProvider)
        {
            this.problemDetailsMiddlewareContextProvider = problemDetailsMiddlewareContextProvider
                ?? throw new System.ArgumentNullException(nameof(problemDetailsMiddlewareContextProvider));
        }

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var middleware = problemDetailsMiddlewareContextProvider.MiddlewareContext;
            middleware.MappableObject = context;

            var problemDetails = ProblemDetailsUtils.CreateMissingMapper(typeof(ApiVersionProblemDetailsMapper));
            middleware.FaultyConditionalResult = new ProblemDetailsResult(problemDetails);

            return new EmptyResult();
        }
    }
}
