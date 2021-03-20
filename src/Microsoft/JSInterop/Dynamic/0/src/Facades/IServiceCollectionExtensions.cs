using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic.Facades.ComponentPropertyAssignments;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicFacadesActivator(IServiceCollection services)
        {
            services.AddJSFacadesActivator();

            ComponentPropertyAssignersUtils.ForEachDefaultComponentPropertyAssigner(defaultComponentPropertyAssignmentType => {
                services.TryAddSingleton<ICollectibleComponentPropertyAssigner>(serviceProvider =>
                    new CollectibleComponentPropertyAssigner(defaultComponentPropertyAssignmentType));
            });

            return services;
        }

        public static IServiceCollection AddJSDynamicFacades(this IServiceCollection services)
        {
            AddJSDynamicFacadesActivator(services);
            services.AddJSFacades();
            return services;
        }
    }
}
