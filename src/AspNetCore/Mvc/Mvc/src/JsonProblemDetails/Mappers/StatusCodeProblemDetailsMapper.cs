// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers.Description;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// A <see cref="ProblemDetails"/> mapper for <see cref="StatusCode"/>.
    /// It's going to be visited from all problem details filters as long
    /// as not handled.
    /// Method <see cref="ProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return <see langword="true"/>.
    /// You may describe <see cref="ProblemDetailsMapperDescriptorOptions.StatusCodes"/>
    /// instead inheriting from this class and overriding <see cref="ProblemDetailsMapper.CanMap"/>.
    /// </summary>
    public class StatusCodeProblemDetailsMapper : ProblemDetailsMapper
    {
        /// <summary>
        /// The status code is set from <see cref="HttpResponse.StatusCode"/>. As we only 
        /// compare to status code, we do not regard <see cref="IMapperContext.StatusCode"/>.
        /// </summary>
        public virtual int StatusCode { get; protected set; }

        public StatusCodeProblemDetailsMapper(IMapperContext mapperContext, ActionExecutedContext context)
            : base(mapperContext) =>
            StatusCode = context.HttpContext.Response.StatusCode;

        public StatusCodeProblemDetailsMapper(IMapperContext mapperContext, ExceptionContext context)
            : base(mapperContext) =>
            StatusCode = context.HttpContext.Response.StatusCode;

        public StatusCodeProblemDetailsMapper(IMapperContext mapperContext, HttpContext context)
            : base(mapperContext) =>
            StatusCode = context.Response.StatusCode;

        public override ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateProblemDetails(statusCode: StatusCode);
    }
}
