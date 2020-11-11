using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Teronis.Mvc.JsonProblemDetails.Reflection;

namespace Teronis.Mvc.JsonProblemDetails.Mappers.Description
{
    public static class ProblemDetailsMapperDescriptorUtils
    {
        private static bool tryCreateMapperConstructorAreaLimitException(MapperConstructorArea mapperConstructorArea, [MaybeNullWhen(false)] out Exception error)
        {
            if (mapperConstructorArea != MapperConstructorArea.Middleware) {
                error = new InvalidOperationException("You cannot have a constructor that is callable by two or more areas.");
                return true;
            }

            error = null;
            return false;
        }

        public static Dictionary<MapperConstructorArea, ReadOnlyCollection<MapperConstructorEvaluation>> FindAreaForEachConstructor(Type mapperType)
        {
            mapperType = mapperType ?? throw new ArgumentNullException(nameof(mapperType));
            var mapperTypeConstructors = mapperType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            var mapperConstructors = mapperTypeConstructors
                .Select(mapperConstructor => {
                    var parameters = mapperConstructor.GetParameters();
                    var mapperConstructorArea = MapperConstructorArea.Middleware;

                    var parameterEvaluations = parameters
                        .OrderBy(x => x.Position)
                        .Select(parameterInfo => {
                            var parameterType = parameterInfo.ParameterType;
                            Type? mappableObjectType = null;

                            var isGenericMapperContextType = new Lazy<bool>(() =>
                                parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IMapperContext<>));

                            var isNonGenericMapperContextType = new Lazy<bool>(() =>
                                !parameterType.IsGenericType && parameterType == typeof(IMapperContext));

                            if (parameterInfo.Position == 0 && (isGenericMapperContextType.Value || isNonGenericMapperContextType.Value)) {
                                if (isGenericMapperContextType.Value) {
                                    mappableObjectType = parameterType.GetGenericArguments()[0];
                                } else {
                                    mappableObjectType = typeof(object);
                                }
                            } else {
                                if (parameterType == typeof(ActionExecutedContext)) {
                                    if (tryCreateMapperConstructorAreaLimitException(mapperConstructorArea, out var error)) {
                                        throw error;
                                    }

                                    mapperConstructorArea = MapperConstructorArea.ActionFilter;
                                } else if (parameterType == typeof(ExceptionContext)) {
                                    if (tryCreateMapperConstructorAreaLimitException(mapperConstructorArea, out var error)) {
                                        throw error;
                                    }

                                    mapperConstructorArea = MapperConstructorArea.ExceptionFilter;
                                } else if (parameterType == typeof(HttpContext)) {
                                    if (tryCreateMapperConstructorAreaLimitException(mapperConstructorArea, out var error)) {
                                        throw error;
                                    }

                                    mapperConstructorArea = MapperConstructorArea.Middleware;
                                }
                            }

                            ParameterEvaluation parameterEvaluation;

                            if (mappableObjectType != null) {
                                parameterEvaluation = new MapperContextParameterEvaluation(parameterInfo, mappableObjectType,
                                    isGenericMapperContextType.Value);
                            } else {
                                parameterEvaluation = new ParameterEvaluation(parameterInfo);
                            }

                            return parameterEvaluation;
                        })
                        .ToList();

                    return new MapperConstructorEvaluation(mapperConstructorArea, mapperConstructor, parameterEvaluations);
                })
                .GroupBy(x => x.MapperConstructorArea)
                .ToDictionary(x => x.Key, x => x.ToList().AsReadOnly());

            return mapperConstructors;
        }
    }
}
