using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    public class MapperContext<ObjectType> : IMapperContext<ObjectType>
    {
        [AllowNull]
        public ObjectType MappableObject { get; }
        public int? StatusCode { get; }
        public ProblemDetailsFactoryScoped ProblemDetailsFactory { get; }

        public MapperContext([AllowNull] ObjectType mappableObject, int? statusCode, ProblemDetailsFactoryScoped problemDetailsFactory)
        {
            MappableObject = mappableObject;
            StatusCode = statusCode;
            ProblemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }
    }
}
