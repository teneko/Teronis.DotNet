// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.MappableObjectResolvers;
using Teronis.Mvc.JsonProblemDetails.Reflection;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class ProblemDetailsResultProvider
    {
        private readonly ProblemDetailsOptions options;
        private readonly ProblemDetailsMapperProvider mapperProvider;
        private readonly ProblemDetailsFactory problemDetailsFactory;

        public ProblemDetailsResultProvider(ProblemDetailsMapperProvider mapperProvider,
            ProblemDetailsFactory problemDetailsFactory, IOptions<ProblemDetailsOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.mapperProvider = mapperProvider ?? throw new ArgumentNullException(nameof(mapperProvider));
            this.problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }

        private int? getStatusCodeOrAlternative(object? mappableObject, int? alternativeStatusCode)
        {
            if (mappableObject is IHasProblemDetailsStatusCode customStatusCodeContainer) {
                return customStatusCodeContainer.StatusCode;
            }

            return alternativeStatusCode;
        }

        private bool tryCreateResult(MapperConstructorArea area, HttpContext httpContext, IEnumerable<ServiceDescriptor> serviceDescriptors, object? mappableObject,
            [MaybeNullWhen(false)] out ProblemDetailsResult result, int? comparableStatusCode = null)
        {
            var mappableObjectType = mappableObject?.GetType();
            var mappableStatusCode = getStatusCodeOrAlternative(mappableObject, null);
            var services = new ServiceCollection();
            var problemDetailsFactoryScoped = new ProblemDetailsFactoryScoped(problemDetailsFactory, httpContext);
            IMapperContext mapperContext;

            if (!(mappableObject is null)) {
                mapperContext = (IMapperContext)MapperContext.CreateFromObject(mappableObject, mappableStatusCode, problemDetailsFactoryScoped);
            } else {
                mapperContext = new MapperContext<object>(mappableObject, mappableStatusCode, problemDetailsFactoryScoped);
            }

            services.AddSingleton(mapperContext); // IMapperContext
            services.Add(serviceDescriptors);
            var serviceProvider = services.BuildServiceProvider();
            var mapperDescriptorIndex = 0;

            while (options.MapperDescriptors.TryFind(mapperDescriptorIndex, comparableStatusCode, out var mapperDescriptorFind)) {
                var mapperDescriptor = mapperDescriptorFind.Value.MapperDescriptor;
                var mapperContructorIndex = 0;

                while (mapperDescriptor.TryFindConstructor(mapperContructorIndex, area, mappableObjectType, out var mapperConstructorFind,
                    globalAllowDerivedMappableObjectTypes: mapperDescriptor.AllowDerivedMapperSourceObjectTypes)) {
                    var mapperConstructor = mapperConstructorFind.Value.ConstructorEvaluation;

                    if (mapperProvider.TryCreateMapper(mapperDescriptor, mapperConstructor, serviceProvider, out var mapper) && mapper.CanMap()) {
                        var problemDetails = mapper.CreateProblemDetails();
                        var problemDetailsResult = new ProblemDetailsResult(problemDetails);
                        result = problemDetailsResult;
                        return true;
                    }

                    mapperContructorIndex = mapperConstructorFind.Value.NextIndex;
                }

                mapperDescriptorIndex = mapperDescriptorFind.Value.NextIndex;
            }

            result = null;
            return false;
        }

        public bool TryCreateResult(ActionExecutedContext actionExecutedContext, [MaybeNullWhen(false)] out ProblemDetailsResult result)
        {
            object? mappableObject = null;
            var actionExecutedContextResult = actionExecutedContext.Result;
            int? comparableStatusCode = null;

            if (!(actionExecutedContextResult is null)) {
                var mappableObjectResolvers = options.MappableObjectResolvers.ToList();
                var mappableObjectResolversCount = mappableObjectResolvers.Count;

                if (mappableObjectResolversCount == 0) {
                    mappableObjectResolvers.Add(new DefaultActionResultObjectResolver());
                }

                var mappableObjectResolversEnumerator = mappableObjectResolvers.GetEnumerator();

                while (mappableObjectResolversEnumerator.MoveNext()) {
                    if (mappableObjectResolversEnumerator.Current.TryResolveObject(actionExecutedContextResult, out var resolvedObject)) {
                        mappableObject = resolvedObject ?? throw new NotSupportedException("Action result object resolvers disallow resolving null values.");
                    }
                }

                if (mappableObject is null) {
                    goto exit;
                }

                if (actionExecutedContextResult is IStatusCodeActionResult statusCodeActionResult) {
                    comparableStatusCode = statusCodeActionResult.StatusCode;
                }
            }

            var serviceDescriptors = new[] { new ServiceDescriptor(actionExecutedContext.GetType(), actionExecutedContext) };
            var httpContext = actionExecutedContext.HttpContext;

            if (tryCreateResult(MapperConstructorArea.ActionFilter, httpContext, serviceDescriptors, mappableObject, out result,
                comparableStatusCode: comparableStatusCode)) {
                return true;
            }

            exit:
            result = null;
            return false;
        }

        public bool TryCreateResult(ExceptionContext exceptionContext, [MaybeNullWhen(false)] out ProblemDetailsResult result)
        {
            var exception = exceptionContext.Exception;
            var serviceDescriptors = new[] { new ServiceDescriptor(typeof(ExceptionContext), exceptionContext) };
            var httpContext = exceptionContext.HttpContext;
            // We expect an exception without any http response manipualtion done so far.
            var comparableStatusCode = default(int?);

            if (tryCreateResult(MapperConstructorArea.ExceptionFilter, httpContext, serviceDescriptors, exception, out var innerResult,
                comparableStatusCode: comparableStatusCode)) {
                result = innerResult;
                return true;
            }

            result = null;
            return false;
        }

        public bool TryCreateResult(HttpContext httpContext, object? mappableObject, [MaybeNullWhen(false)] out ProblemDetailsResult result)
        {
            var serviceDescriptors = new[] { new ServiceDescriptor(typeof(HttpContext), httpContext) };
            // Http response can have a status code set by any middleware, filter or action 
            // executor without starting the response actually.
            var comparableStatusCode = httpContext.Response.StatusCode;

            if (tryCreateResult(MapperConstructorArea.Middleware, httpContext, serviceDescriptors, mappableObject, out var innerResult,
                comparableStatusCode: comparableStatusCode)) {
                result = innerResult;
                return true;
            }

            result = null;
            return false;
        }
    }
}
