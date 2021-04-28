// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.AspNetCore.Mvc
{
    public class MvcBuilderContextCreator : IPostConfigureOptions<MvcOptions>
    {
        private readonly IServiceCollection services;
        private readonly Action<MvcBuilderContext> configureContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureContext">Only run once. Not added to service collection.</param>
        public MvcBuilderContextCreator(IServiceCollection services, Action<MvcBuilderContext> configureContext)
        {
            this.services = services;
            this.configureContext = configureContext;
        }

        public void PostConfigure(string name, MvcOptions option)
        {
            var partManager = services.BuildServiceProvider().GetRequiredService<ApplicationPartManager>();
            var contextOptions = new MvcBuilderContext(partManager);
            configureContext(contextOptions);
        }
    }
}
