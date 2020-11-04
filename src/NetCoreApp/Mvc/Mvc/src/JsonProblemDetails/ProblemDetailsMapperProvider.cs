using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Teronis.Mvc.JsonProblemDetails.Descriptor;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.Reflection;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class ProblemDetailsMapperProvider
    {
        private readonly IServiceProvider applicationServiceProvider;

        public ProblemDetailsMapperProvider(IServiceProvider applicationServiceProvider) =>
            this.applicationServiceProvider = applicationServiceProvider ?? throw new ArgumentNullException(nameof(applicationServiceProvider));

        public bool TryCreateMapper(ProblemDetailsMapperDescriptor descriptor, MapperConstructorEvaluation mapperConstructor, IServiceProvider problemDetailsServiceProvider,
            [MaybeNullWhen(false)] out IProblemDetailsMapper mapper)
        {
            descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            var mapperType = descriptor.MapperType;
            var parameterEvaluations = mapperConstructor.ParameterEvaluations;

            var constructorArguments = parameterEvaluations.Select(parameterEvaluation => {
                object? parameterValue = null;

                if (parameterEvaluation.TryGetMapperContextParameterEvaluation(out _)) {
                    parameterValue = problemDetailsServiceProvider.GetService(typeof(IMapperContext));
                } else {
                    var parameterType = parameterEvaluation.SourceInfo.ParameterType;

                    if (parameterType == typeof(ActionExecutedContext)
                        || parameterType == typeof(ExceptionContext)
                        || parameterType == typeof(HttpContext)) {
                        parameterValue = problemDetailsServiceProvider.GetService(parameterType);
                    }

                    parameterValue ??= applicationServiceProvider.GetService(parameterType);
                }

                return parameterValue;
            }).ToArray();

            if (!constructorArguments.Any(x => x is null)) {
                mapper = (IProblemDetailsMapper)mapperConstructor.SourceInfo.Invoke(constructorArguments);
                return true;
            }

            mapper = null;
            return false;
        }
    }
}
