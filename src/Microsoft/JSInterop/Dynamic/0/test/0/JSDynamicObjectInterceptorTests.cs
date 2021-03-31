// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception;
using Teronis.Microsoft.JSInterop.Dynamic.ObjectReferences;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Interceptors;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectInterceptorTests : JSDynamicObjectTestsBase
    {
        private readonly IServiceProvider serviceProvider;

        public JSDynamicObjectInterceptorTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IJSRuntime, TestRuntime>();
            services.AddJSDynamicFacadeHub();

            services.AddJSDynamicProxyActivator(configureInterceptorBuilderOptions: options => 
                options.ConfigureInterceptorBuilder(builder =>
                    builder.Add(typeof(JSDynamicProxyActivatingInterceptor))));

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task Should_create_operable_nested_proxy()
        {
            var nestedOwningObjectReference = new NestedOwningObjectReference();
            var facadeHub = serviceProvider.GetRequiredService<IJSFacadeHub<JSDynamicFacadeActivators>>();
            var dynamicProxyActivator = facadeHub.Activators.JSDynamicProxyActivator;

            {
                await using var nestedOwningObject = dynamicProxyActivator.CreateInstance<INestedOwningDynamicObject>(nestedOwningObjectReference);
                await using var nestedObject = await nestedOwningObject.GetNestedObjectAsync();

                // Act
                var identifier = await nestedObject.ReturnsThisNameAsync();
                await facadeHub.DisposeAsync();

                // Assert
                Assert.Equal(nameof(INestedDynamicObject.ReturnsThisNameAsync), identifier);
            }

            Assert.True(nestedOwningObjectReference.IsDisposed);
            Assert.True(nestedOwningObjectReference.ObjectReferences[0].IsDisposed);
        }
    }
}
