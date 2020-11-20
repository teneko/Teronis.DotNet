using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Mvc.Hosting
{
    public static class IServiceCollectionExtensions
    {
        public static void PostConfigureMvcBuilderContext(this IServiceCollection services, Action<MvcBuilderContext> configurePartManager)
        {
            services.AddOptions();
            services.ConfigureOptions(new MvcBuilderContextCreator(services, configurePartManager));
        }
    }
}
