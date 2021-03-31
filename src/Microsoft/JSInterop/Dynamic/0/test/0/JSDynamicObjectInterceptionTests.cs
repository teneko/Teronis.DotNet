// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Activators;
using Teronis.Microsoft.JSInterop.Dynamic.Interceptors;
using Teronis.Microsoft.JSInterop.Dynamic.Interefaces.Interception;
using Teronis.Microsoft.JSInterop.Dynamic.ObjectReferences;
using Teronis.Microsoft.JSInterop.Facades;
using Xunit;
using Xunit.Repeat;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectInterceptionTests : JSDynamicObjectTestsBase
    {
        //[Fact]
        [Theory]
        [Repeat(10)]
        public async Task Should_create_operable_nested_proxy(int iterationNumber)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IJSRuntime, TestRuntime>();
            services.AddJSDynamicFacadeHub();

            services.AddJSDynamicProxyActivator(options => options.ConfigureInterceptorBuilder(builder =>
                builder.Add(typeof(JSDynamicProxyActivatingInterceptor))));

            var serviceProvider = services.BuildServiceProvider();
            var jsObjectReference = new NestedOwningObjectReference();
            var facadeHub = serviceProvider.GetRequiredService<IJSFacadeHubActivator>().CreateInstance<JSDynamicFacadeActivators>();
            var activator = facadeHub.Activators.JSDynamicProxyActivator;
            var nestedOwningObject = activator.CreateInstance<INestedOwningDynamicObject>(jsObjectReference);
            var nestedObject = await nestedOwningObject.GetNestedObjectAsync();

            // Act
            var identifier = await nestedObject.ReturnsThisNameAsync();
            await facadeHub.DisposeAsync();

            // Assert
            Assert.Equal(nameof(INestedDynamicObject.ReturnsThisNameAsync), identifier);
            Assert.Equal(2, facadeHub.Disposables.Count);
            Assert.All(jsObjectReference.ObjectReferences, objectReference => Assert.True(objectReference.IsDisposed));
        }
    }
}
