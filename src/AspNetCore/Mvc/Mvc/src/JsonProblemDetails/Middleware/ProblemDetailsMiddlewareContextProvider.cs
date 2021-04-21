// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public class ProblemDetailsMiddlewareContextProvider
    {
        public ProblemDetailsMiddlewareContext MiddlewareContext {
            get => middlewareContext
                ?? throw new InvalidOperationException($"The scoped service ({nameof(ProblemDetailsMiddlewareContext)}) has been not set during middleware pipeline.");

            set => middlewareContext = value;
        }

        private ProblemDetailsMiddlewareContext? middlewareContext;
    }
}
