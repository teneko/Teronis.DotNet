using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectOptionsConfiguration : IConfigureOptions<JSFunctionalObjectOptions>
    {
        public static JSFunctionalObjectOptionsConfiguration Create(IServiceProvider serviceProvider) =>
            ActivatorUtilities.CreateInstance<JSFunctionalObjectOptionsConfiguration>(serviceProvider);

        private readonly IServiceProvider serviceProvider;

        public JSFunctionalObjectOptionsConfiguration(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Configure(JSFunctionalObjectOptions options) {
            options.ServiceProvider = serviceProvider;
        }
    }
}
