// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Teronis.Mvc.JsonProblemDetails.Middleware;

namespace Teronis.Mvc.JsonProblemDetails
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Use it to add <see cref="ProblemDetailsMiddleware"/> 
        /// to pipeline. Place it as early as possible.
        /// </summary>
        /// <param name="builder">The builder where middleware is about to be added.</param>
        /// <returns>Same builder you passed through.</returns>
        public static IApplicationBuilder UseProblemDetails(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ProblemDetailsMiddleware>();
            return builder;
        }
    }
}
