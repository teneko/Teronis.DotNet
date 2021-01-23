using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.JsonProblemDetails
{
    public static class IServiceCollectionExtensions
    {
        public static IJsonProblemDetailsBuilder CreateJsonProblemDetailsBuilder(this IServiceCollection services) =>
            new JsonProblemDetailsBuilder(services);
    }
}

//using System;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.AspNetCore.Mvc.Versioning;
//using Microsoft.Extensions.DependencyInjection;
//using Teronis.Mvc.JsonProblemDetails.Filters;
//using Teronis.Mvc.JsonProblemDetails.Middleware;
//using Teronis.Mvc.JsonProblemDetails.Versioning;

//namespace Teronis.Mvc.JsonProblemDetails
//{
//    public static class IServiceCollectionExtensions
//    {
//        public static IServiceCollection AddJsonProblemDetails(this IServiceCollection services, Action<ProblemDetailsOptions>? configureOptions = null)
//        {
//            if (configureOptions != null) {
//                services.Configure(configureOptions);
//            }

//            services.PostConfigure<MvcOptions>(options => {
//                options.Filters.Add<ProblemDetailsActionFilter>();
//                options.Filters.Add<ProblemDetailsExceptionFilter>();
//            });

//            services.AddSingleton<ProblemDetailsMiddlewareContextProxy>();
//            services.AddScoped<ProblemDetailsMiddlewareContext>();

//            services.AddOptions<ApiBehaviorOptions>().Configure<ProblemDetailsMiddlewareContextProxy>((options, problemDetailsMiddlewareContextProxy) => {
//                options.SuppressMapClientErrors = true;
//                options.SuppressModelStateInvalidFilter = false;

//                options.InvalidModelStateResponseFactory = (actionContext) => {
//                    problemDetailsMiddlewareContextProxy.MiddlewareContext.SetMappableObject = actionContext.ModelState;
//                    return new EmptyResult();
//                };
//            });

//            services.AddSingleton<ProblemDetailsMapperProvider>();
//            services.AddSingleton<ProblemDetailsResponseProvider>();
//            services.AddSingleton<IActionResultExecutor<ProblemDetailsResult>, ProblemDetailsResultExecutor>();
//            return services;
//        }

//        public static IServiceCollection AddApiVersioningProblemDetails(this IServiceCollection services)
//        {
//            services.AddOptions<ApiVersioningOptions>().Configure<ProblemDetailsMiddlewareContextProxy>((options, problemDetailsMiddlewareContextProxy) => {
//                options.ErrorResponses = new ApiVersionProblemDetailsResponseProvider(problemDetailsMiddlewareContextProxy);
//            });

//            return services;
//        }
//    }
//}
