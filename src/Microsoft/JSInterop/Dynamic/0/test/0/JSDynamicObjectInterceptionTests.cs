// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Dynamic.Interceptors;
using Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception;
using Teronis.Microsoft.JSInterop.Dynamic.ObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectInterceptionTests : JSDynamicObjectTestsBase
    {
        [Fact]
        public async Task Should_create_functional_nested_proxy()
        {
            var services = new ServiceCollection();
            services.AddJSDynamicProxy();

            services.AddJSDynamicProxyActivator(options => options.ConfigureInterceptorWalkerBuilder(builder =>
                builder.AddInterceptor(typeof(JSDynamicProxyActivatingInterceptor))));

            var serviceProvider = services.BuildServiceProvider();
            var jsObjectReference = new NestedOwningObjectReference();

            var activator = serviceProvider.GetRequiredService<IJSDynamicProxyActivator>();
            var nestedOwningObject = activator.CreateInstance<INestedOwningDynamicObject>(jsObjectReference);
            var nestedObject = await nestedOwningObject.GetNestedObjectAsync();

            // Act
            var identifier = await nestedObject.ReturnsThisNameAsync();

            // Assert
            Assert.Equal(nameof(INestedDynamicObject.ReturnsThisNameAsync), identifier);
        }
    }
}
