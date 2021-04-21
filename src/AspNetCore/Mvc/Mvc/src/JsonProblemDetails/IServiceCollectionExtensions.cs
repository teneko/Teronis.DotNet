// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.JsonProblemDetails
{
    public static class IServiceCollectionExtensions
    {
        public static IJsonProblemDetailsBuilder CreateJsonProblemDetailsBuilder(this IServiceCollection services) =>
            new JsonProblemDetailsBuilder(services);
    }
}
