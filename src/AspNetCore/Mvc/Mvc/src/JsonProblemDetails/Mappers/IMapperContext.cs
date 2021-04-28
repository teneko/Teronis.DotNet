// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers
{
    public interface IMapperContext
    {
        /// <summary>
        /// This is the status code that has been provided by
        /// <see cref="IHasProblemDetailsStatusCode.StatusCode"/>.
        /// </summary>
        public int? StatusCode { get; }
        public ProblemDetailsFactoryScoped ProblemDetailsFactory { get; }
    }
}
