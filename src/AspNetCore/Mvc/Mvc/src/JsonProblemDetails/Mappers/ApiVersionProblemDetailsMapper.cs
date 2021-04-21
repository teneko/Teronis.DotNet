// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    public class ApiVersionProblemDetailsMapper : ProblemDetailsMapper<ErrorResponseContext>
    {
        public ApiVersionProblemDetailsMapper(IMapperContext<ErrorResponseContext> mapperContext)
            : base(mapperContext) { }

        public override ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateProblemDetails(
                statusCode: MapperContext.MappableObject.StatusCode,
                title: MapperContext.MappableObject.Message,
                type: MapperContext.MappableObject.ErrorCode,
                detail: MapperContext.MappableObject.MessageDetail);
    }
}
