// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    public class DefaultExceptionProblemDetailsMapper : ExceptionProblemDetailsMapper<Exception>
    {
        public DefaultExceptionProblemDetailsMapper([AllowInheritances] IMapperContext<Exception> mapperContext, ExceptionContext exceptionContext)
            : base(mapperContext, exceptionContext) { }
    }
}
