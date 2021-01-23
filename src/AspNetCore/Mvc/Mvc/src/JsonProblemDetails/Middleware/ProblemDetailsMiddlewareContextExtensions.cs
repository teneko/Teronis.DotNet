namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public static class ProblemDetailsMiddlewareContextExtensions
    {
        public static bool CanSkipFilter(this ProblemDetailsMiddlewareContext middlewareContext) =>
            middlewareContext.Handled || !(middlewareContext.MappableObject is null);
    }
}
