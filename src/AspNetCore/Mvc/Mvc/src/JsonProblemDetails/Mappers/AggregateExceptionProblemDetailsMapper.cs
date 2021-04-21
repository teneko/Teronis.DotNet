// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    public class AggregateExceptionProblemDetailsMapper : ExceptionProblemDetailsMapper<KeyedAggregateException>
    {
        public AggregateExceptionProblemDetailsMapper(IMapperContext<KeyedAggregateException> mapperContext, ExceptionContext exceptionContext)
            : base(mapperContext, exceptionContext) { }

        public override ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateValidationProblemDetails(MapperContext.MappableObject,
                statusCode: MapperContext.StatusCode);
    }
}
