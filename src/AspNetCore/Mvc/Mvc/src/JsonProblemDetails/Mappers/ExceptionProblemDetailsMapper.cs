using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Teronis.Extensions;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    /// <typeparam name="MappableObjectType"></typeparam>
    public class ExceptionProblemDetailsMapper<TException> : ProblemDetailsMapper<TException>
        where TException : Exception
    {
        public ExceptionProblemDetailsMapper(IMapperContext<TException> mapperContext, ExceptionContext _)
            : base(mapperContext) { }

        public override ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateProblemDetails(MapperContext.MappableObject,
                statusCode: MapperContext.StatusCode);
    }
}
