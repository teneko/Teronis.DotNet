// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    public abstract class ProblemDetailsMapper : IProblemDetailsMapper
    {
        public IMapperContext MapperContext { get; }

        public ProblemDetailsMapper(IMapperContext mapperContext)
        {
            MapperContext = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));
        }

        public virtual bool CanMap() => true;

        public virtual ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateProblemDetails();
    }
}
