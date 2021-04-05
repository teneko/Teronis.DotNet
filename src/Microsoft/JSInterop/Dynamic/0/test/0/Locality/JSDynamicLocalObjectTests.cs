// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Locality.DynamicObjects;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSDynamicLocalObjectTests
    {
        [Fact]
        public async Task Get_local_object()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJSRuntime, TestJavaScriptRuntime>()
                .AddSingleton<IJSLocalObjectActivator, EmptyLocalObjectActivator>()
                .AddJSDynamicProxy()
                .Configure<GlobalValueAssignerOptions>(options =>
                    _ = options.Services)
                .AddJSDynamicProxyActivator(
                    configureInterceptorBuilderOptions: options =>
                        options.ConfigureInterceptorServices(services =>
                            services.UseExtension(extension =>
                                extension.AddScoped<JSLocalObjectInterceptor>())))
                .AddJSDynamicLocalObject()
                .BuildServiceProvider();

            var dynamicProxyActivator = serviceProvider.GetRequiredService<IJSDynamicProxyActivator>();
            var dynamicProxy = dynamicProxyActivator.CreateInstance<ILocalObjectReturningDynamicObject>(new EmptyObjectReference());

            // Act
            var localObject = await dynamicProxy.GetLocalObject();

            // Assert
            var emptyObjectReference = Assert.IsType<EmptyObjectReference>(localObject.ObjectReference);
            Assert.Equal(nameof(EmptyLocalObjectActivator), emptyObjectReference.Tag);
        }

        [Fact]
        public async Task Get_dynamic_local_object()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJSRuntime, TestJavaScriptRuntime>()
                .AddSingleton<IJSLocalObjectInterop, EmptyLocalObjectInterop>()
                .AddJSDynamicProxy()
                .Configure<GlobalValueAssignerOptions>(options =>
                    _ = options.Services)
                .AddJSLocalObjectActivator(
                    configureInterceptorBuilderOptions: options =>
                        options.ConfigureInterceptorServices(services =>
                            services.UseExtension(extension =>
                                extension.AddScoped<JSLocalObjectInterceptor>())))
                .AddJSDynamicProxyActivator(
                    configureInterceptorBuilderOptions: options =>
                        options.ConfigureInterceptorServices(services =>
                            services.UseExtension(extension =>
                                extension.AddScoped<JSDynamicLocalObjectInterceptor>())))
                .AddJSDynamicLocalObject()
                .BuildServiceProvider();

            var dynamicProxyActivator = serviceProvider.GetRequiredService<IJSDynamicProxyActivator>();
            var dynamicProxy = dynamicProxyActivator.CreateInstance<IDynamicLocalObjectReturningDynamicObject>(new EmptyObjectReference());

            // Act
            var dynamicLocalObject = await dynamicProxy.GetDynamicLocalObject();
            var localObject = await dynamicLocalObject.GetLocalObject();

            // Assert
            var emptyObjectReference = Assert.IsType<EmptyObjectReference>(localObject.ObjectReference);
            Assert.Equal(nameof(EmptyLocalObjectInterop), emptyObjectReference.Tag);
        }
    }
}
