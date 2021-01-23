using System;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    /// <typeparam name="MappableObjectType"></typeparam>
    public abstract class ProblemDetailsMapper<MappableObjectType> : IProblemDetailsMapper
    {
        public IMapperContext<MappableObjectType> MapperContext { get; }

        public ProblemDetailsMapper(IMapperContext<MappableObjectType> mapperContext) =>
            MapperContext = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));

        public virtual bool CanMap() => true;
        public abstract ProblemDetails CreateProblemDetails();
    }
}
