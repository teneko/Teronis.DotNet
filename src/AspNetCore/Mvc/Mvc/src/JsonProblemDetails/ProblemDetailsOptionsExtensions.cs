// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers.Description;
using Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
{
    public static class ProblemDetailsOptionsExtensions
    {
        public static ProblemDetailsOptions AddDefaultMappers(this ProblemDetailsOptions options) {
            options.MapperDescriptors.Add<ModelStateProblemDetailsMapper>();
            options.MapperDescriptors.Add<ApiVersionProblemDetailsMapper>();
            options.MapperDescriptors.Add<AggregateExceptionProblemDetailsMapper>();
            options.MapperDescriptors.Add<DefaultExceptionProblemDetailsMapper>();

            options.MapperDescriptors.Add<StatusCodeProblemDetailsMapper>(new ProblemDetailsMapperDescriptorOptions()
                .WithStatusCodeRange(400, 599));

            return options;
        }
    }
}
