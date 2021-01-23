using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    /// <summary>
    /// Method <see cref="IProblemDetailsMapper.CanMap"/> is defaultly 
    /// implemented to return true.
    /// </summary>
    /// <typeparam name="MappableObjectType"></typeparam>
    public class ModelStateProblemDetailsMapper : ProblemDetailsMapper<ModelStateDictionary>
    {
        public ModelStateProblemDetailsMapper(IMapperContext<ModelStateDictionary> mapperContext)
            : base(mapperContext) { }

        public override ProblemDetails CreateProblemDetails() =>
            MapperContext.ProblemDetailsFactory.CreateValidationProblemDetails(MapperContext.MappableObject);
    }
}
