using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.ApplicationModels
{
    public static class IServiceCollectionExtensions
    {
        private static void configureControllerModel(ControllerModelConfiguration controllerModelConfiguration,
            Action<Action<MvcOptions>> configureOptions) =>
            configureOptions(new MvcOptionsConfigurator(controllerModelConfiguration).ConfigureMvcOptions);

        public static IServiceCollection ConfigureControllerModel(this IServiceCollection services,
            ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.Configure(configurator));

            return services;
        }

        public static IServiceCollection ConfigureControllerModel(this IServiceCollection services,
            string name, ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.Configure(name, configurator));

            return services;
        }

        public static IServiceCollection ConfigureAllControllerModel(this IServiceCollection services,
            ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.ConfigureAll(configurator));

            return services;
        }

        public static IServiceCollection PostConfigureControllerModel(this IServiceCollection services,
            ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.PostConfigure(configurator));

            return services;
        }

        public static IServiceCollection PostConfigureControllerModel(this IServiceCollection services,
            string name, ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.PostConfigure(name, configurator));

            return services;
        }

        public static IServiceCollection PostConfigureAllControllerModel(this IServiceCollection services,
            ControllerModelConfiguration controllerModelConfiguration)
        {
            configureControllerModel(controllerModelConfiguration,
                configurator => services.PostConfigureAll(configurator));

            return services;
        }

        private class MvcOptionsConfigurator
        {
            private readonly ControllerModelConfiguration controllerModelConfiguration;

            public MvcOptionsConfigurator(ControllerModelConfiguration controllerModelConfiguration) =>
                this.controllerModelConfiguration = controllerModelConfiguration;

            public void ConfigureMvcOptions(MvcOptions options)
            {
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
            }
        }
    }
}
