// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptorTests
    {
        [Fact]
        public async Task Should_intercept_in_order()
        {
            var numbers = new List<int>();

            var jsInterceptor = new JSInterceptorBuilder()
                .Add(new JSCallingBackInterceptor(
                    (invocation, context) => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackInterceptor(
                    (invocation, context) => {
                        numbers.Add(1);
                        return ValueTask.CompletedTask;
                    }))
                .Build(serviceProvider: null);

            // Act
            await jsInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }

        public async Task Should_cancel_interception()
        {
            var numbers = new List<int>();

            var jsInterceptor = new JSInterceptorBuilder()
                .Add(new JSCallingBackInterceptor(
                    (invocation, context) => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackInterceptor(
                    (invocation, context) => {
                        numbers.Add(1);
                        context.StopInterception();
                        return ValueTask.CompletedTask;
                    }))
                .Add(new JSCallingBackInterceptor(
                    (invocation, context) => {
                        numbers.Clear();
                        return ValueTask.CompletedTask;
                    }))
                .Build(serviceProvider: null);

            // Act
            await jsInterceptor.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }
    }
}
