using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Mvc
{
    public class MvcBuilderContextCreator : IPostConfigureOptions<MvcOptions>
    {
        private readonly IServiceCollection services;
        private readonly Action<MvcBuilderContext> configureOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureContext">Only run once. Not added to service collection.</param>
        public MvcBuilderContextCreator(IServiceCollection services, Action<MvcBuilderContext> configureContext)
        {
            this.services = services;
            this.configureOptions = configureContext;
        }

        public void PostConfigure(string name, MvcOptions option)
        {
            var partManager = services.BuildServiceProvider().GetRequiredService<ApplicationPartManager>();
            var contextOptions = new MvcBuilderContext(partManager);
            configureOptions(contextOptions);
        }
    }
}
