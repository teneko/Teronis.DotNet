using System;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public class ProblemDetailsMiddlewareContext
    {
        public object? MappableObject { get; set; }
        /// <summary>
        /// Only taken when <see cref="MappableObject"/> cannot be mapped..
        /// </summary>
        public ProblemDetailsResult? FaultyConditionalResult { get; set; }
        
        public bool Handled { get; set; }

        public bool CanSkipFilter() =>
            Handled || !(MappableObject is null);
    }
}
