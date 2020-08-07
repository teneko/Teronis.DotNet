using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.ApplicationModels
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureControllerModel(this IServiceCollection services, ControllerModelConfiguration controllerModelConfiguration)
        {
            services.Configure<MvcOptions>(options => {
                foreach (var controllerConvention in controllerModelConfiguration.ControllerConventions) {
                    options.Conventions.Add(controllerConvention);
                }

                foreach (var actionConvention in controllerModelConfiguration.ActionConventions) {
                    options.Conventions.Add(actionConvention);
                }

                foreach (var parameterConvention in controllerModelConfiguration.ParameterConventions) {
                    options.Conventions.Add(parameterConvention);
                }

                foreach (var parameterBaseConvention in controllerModelConfiguration.ParameterBaseConventions) {
                    options.Conventions.Add(parameterBaseConvention);
                }
            });

            return services;
        }
    }
}
