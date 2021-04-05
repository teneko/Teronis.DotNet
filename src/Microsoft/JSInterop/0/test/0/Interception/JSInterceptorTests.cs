// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;
using Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptorTests
    {
        private readonly IServiceProvider serviceProvider;

        public JSInterceptorTests() =>
            serviceProvider = new ServiceCollection().BuildServiceProvider();

        [Fact]
        public async Task Should_intercept_in_order()
        {
            var numbers = new List<int>();

            var jsInterceptor = new JSInterceptorServiceCollection()
                .UseExtension(extension => {
                    extension.AddScoped(sp => new JSCallingBackInterceptor(
                        (invocation, context) => {
                            numbers.Add(0);
                            return ValueTask.CompletedTask;
                        }));

                    extension.AddScoped(sp => new JSCallingBackInterceptor(
                        (invocation, context) => {
                            numbers.Add(1);
                            return ValueTask.CompletedTask;
                        }));
                })
                .Build(serviceProvider);

            // Act
            await jsInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }

        public async Task Should_cancel_interception()
        {
            var numbers = new List<int>();

            var jsInterceptor = new JSInterceptorServiceCollection()
                .UseExtension(extension => {
                    extension.AddScoped(sp => new JSCallingBackInterceptor(
                        (invocation, context) => {
                            numbers.Add(0);
                            return ValueTask.CompletedTask;
                        }));

                    extension.AddScoped(sp => new JSCallingBackInterceptor(
                        (invocation, context) => {
                            numbers.Add(1);
                            context.StopInterception();
                            return ValueTask.CompletedTask;
                        }));

                    extension.AddScoped(sp => new JSCallingBackInterceptor(
                        (invocation, context) => {
                            numbers.Clear();
                            return ValueTask.CompletedTask;
                        }));
                })
                .Build(serviceProvider);

            // Act
            await jsInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }
    }
}
