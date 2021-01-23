using System;

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public class ProblemDetailsMiddlewareContextProxy
    {
        public ProblemDetailsMiddlewareContext MiddlewareContext {
            get => middlewareContext 
                ?? throw new InvalidOperationException($"The scoped service ({nameof(ProblemDetailsMiddlewareContext)}) has been not set during middleware pipeline.");

            set => middlewareContext = value;
        }

        private ProblemDetailsMiddlewareContext? middlewareContext;
    }
}
