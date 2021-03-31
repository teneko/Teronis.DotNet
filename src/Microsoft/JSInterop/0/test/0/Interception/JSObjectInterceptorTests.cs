// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Interception;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Interceptions
{
    public class JSObjectInterceptorTests
    {
        [Fact]
        public async Task Should_intercept_in_order()
        {
            var numbers = new List<int>();

            var jsObjectInterceptor = new JSIteratingInterceptorBuilder()
                .Add(new JSCallingBackObjectInterceptor(
                    invocation => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackObjectInterceptor(
                    invocation => {
                        numbers.Add(1);
                        return ValueTask.CompletedTask;
                    }))
                .Build(serviceProvider: null);

            // Act
            await jsObjectInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }

        public async Task Should_cancel_interception() {
            var numbers = new List<int>();

            var jsObjectInterceptor = new JSIteratingInterceptorBuilder()
                .Add(new JSCallingBackObjectInterceptor(
                    invocation => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackObjectInterceptor(
                    invocation => {
                        numbers.Add(1);
                        invocation.StopInterception();
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackObjectInterceptor(
                    invocation => {
                        numbers.Clear();
                        return ValueTask.CompletedTask;
                    }))
                .Build(serviceProvider: null);

            // Act
            await jsObjectInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }
    }
}
