using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Mvc.JsonProblemDetails.Filters;
using Teronis.Mvc.JsonProblemDetails.Mappers;
using Teronis.Mvc.JsonProblemDetails.Middleware;
using Teronis.Mvc.JsonProblemDetails.Versioning;
using ApiVersioningServiceCollectionExtensions = Microsoft.Extensions.DependencyInjection.IServiceCollectionExtensions;

namespace Teronis.Mvc.JsonProblemDetails
{
    public static class IJsonProblemDetailsBuilderExtensions
    {
        /// <summary>
        /// Adds the problem details services to <see cref="IJsonProblemDetailsBuilder.Services"/>. 
        /// The filters <see cref="ProblemDetailsActionFilter"/> and <see cref="ProblemDetailsExceptionFilter"/>
        /// are added to <see cref="MvcOptions.Filters"/> via 
        /// <see cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// This means the order of configuring <see cref="MvcOptions"/> matters.
        /// </summary>
        public static IJsonProblemDetailsBuilder AddJsonProblemDetails(this IJsonProblemDetailsBuilder builder, Action<ProblemDetailsOptions>? configureOptions = null)
        {
            var services = builder.Services ?? throw new ArgumentException();
            
            if (configureOptions != null) {
                services.Configure(configureOptions);
            }

            services.Configure<MvcOptions>(options => {
                options.Filters.Add<ProblemDetailsActionFilter>();
                options.Filters.Add<ProblemDetailsExceptionFilter>();
            });

            services.AddSingleton<ProblemDetailsMiddlewareContextProxy>();
            services.AddScoped<ProblemDetailsMiddlewareContext>();
            services.AddSingleton<ProblemDetailsMapperProvider>();
            services.AddSingleton<ProblemDetailsResultProvider>();
            services.AddSingleton<IActionResultExecutor<ProblemDetailsResult>, ProblemDetailsResultExecutor>();
            return builder;
        }

        /// <summary>
        /// This methods adds json problem details abilities to <see cref="ApiBehaviorOptions"/> via
        /// <see cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// This means the order of configuring <see cref="ApiBehaviorOptions"/> matters. You have to call it after
        /// calling <see cref="MvcCoreServiceCollectionExtensions.AddMvcCore(IServiceCollection)"/>,
        /// <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection)"/> or similiar.
        /// </summary>
        public static IJsonProblemDetailsBuilder ConfigureApiBehaviourOptions(this IJsonProblemDetailsBuilder builder)
        {
            var services = builder.Services ?? throw new ArgumentException();
            
            services.AddOptions<ApiBehaviorOptions>().Configure<ProblemDetailsMiddlewareContextProxy>((options, problemDetailsMiddlewareContextProxy) => {
                options.SuppressMapClientErrors = true;
                options.SuppressModelStateInvalidFilter = false;

                options.InvalidModelStateResponseFactory = (actionContext) => {
                    var middleware = problemDetailsMiddlewareContextProxy.MiddlewareContext;
                    middleware.MappableObject = actionContext.ModelState;
                    var problemDetails = ProblemDetailsUtils.CreateMissingMapper(typeof(ApiVersionProblemDetailsMapper));
                    middleware.FaultyConditionalResult = new ProblemDetailsResult(problemDetails);
                    return new EmptyResult();
                };
            });

            return builder;
        }

        /// <summary>
        /// This methods adds json problem details abilities to <see cref="ApiVersioningOptions"/> via 
        /// <see cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// This means the order of configuring <see cref="ApiVersioningOptions"/> matters. You have to call it after
        /// <see cref="ApiVersioningServiceCollectionExtensions.AddApiVersioning(IServiceCollection)"/>.
        /// </summary>
        public static IJsonProblemDetailsBuilder ConfigureApiVersioningOptions(this IJsonProblemDetailsBuilder builder)
        {
            var services = builder.Services ?? throw new ArgumentException();

            services.AddOptions<ApiVersioningOptions>().Configure<ProblemDetailsMiddlewareContextProxy>((options, problemDetailsMiddlewareContextProxy) => {
                options.ErrorResponses = new ApiVersionProblemDetailsResponseProvider(problemDetailsMiddlewareContextProxy);
            });

            return builder;
        }
    }
}
