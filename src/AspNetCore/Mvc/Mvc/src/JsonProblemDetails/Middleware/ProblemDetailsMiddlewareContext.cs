// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc.Filters;

namespace Teronis.Mvc.JsonProblemDetails.Middleware
{
    public class ProblemDetailsMiddlewareContext
    {
        /// <summary>
        /// The result that has been written to response.
        /// </summary>
        public ProblemDetailsResult? HandledResult { get; private set; }
        /// <summary>
        /// <see cref="true"/> if <see cref="HandledResult"/> is not null.
        /// </summary>
        public bool Handled => !(HandledResult is null);

        /// <summary>
        /// If not null the object is going to to be
        /// expected  definitely being mapped. Otherwise
        /// <see cref="FaultyConditionalResult"/> or
        /// erroneous default result will be taken as
        /// response.
        /// </summary>
        public object? MappableObject { get; set; }
        /// <summary>
        /// Only taken when <see cref="MappableObject"/> cannot be mapped.
        /// </summary>
        public ProblemDetailsResult? FaultyConditionalResult { get; set; }

        /// <summary>
        /// You may call this from a <see cref="IActionFilter"/> 
        /// or <see cref="IExceptionFilter"/>.
        /// </summary>
        /// <returns><see cref="true"/> if filter is skippable.</returns>
        public bool IsFilterSkippable() =>
            Handled || !(MappableObject is null);

        public void SetHandled(ProblemDetailsResult? result) =>
            HandledResult = result;
    }
}
