using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    /// <typeparam name="MappableObjectType"></typeparam>
    public class DefaultExceptionProblemDetailsMapper : ExceptionProblemDetailsMapper<Exception>
    {
        public DefaultExceptionProblemDetailsMapper([AllowInheritances] IMapperContext<Exception> mapperContext, ExceptionContext exceptionContext)
            : base(mapperContext, exceptionContext) { }
    }
}
