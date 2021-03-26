// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public static class ProblemDetailsMiddlewareContextExtensions
    {
        public static bool CanSkipFilter(this ProblemDetailsMiddlewareContext middlewareContext) =>
            middlewareContext.Handled || !(middlewareContext.MappableObject is null);
    }
}
