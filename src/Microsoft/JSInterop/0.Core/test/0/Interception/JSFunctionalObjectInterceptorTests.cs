// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Interception;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Interceptions
{
    public class JSFunctionalObjectInterceptorTests
    {
        [Fact]
        public async Task Should_intercept_in_order()
        {
            var numbers = new List<int>();

            var jsFunctionalObject = new JSFunctionalObjectInterceptorWalkerBuilder()
                .AddInterceptor(new JSCallingBackFunctionalObjectInterceptor(
                    invocation => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .AddInterceptor(new JSCallingBackFunctionalObjectInterceptor(
                    invocation => {
                        numbers.Add(1);
                        return ValueTask.CompletedTask;
                    }))
                .BuildInterceptableFunctionalObject();

            // Act
            await jsFunctionalObject.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }

        public async Task Should_cancel_interception() {
            var numbers = new List<int>();

            var jsFunctionalObject = new JSFunctionalObjectInterceptorWalkerBuilder()
                .AddInterceptor(new JSCallingBackFunctionalObjectInterceptor(
                    invocation => {
                        numbers.Add(0);
                        return ValueTask.CompletedTask;
                    }))
                .AddInterceptor(new JSCallingBackFunctionalObjectInterceptor(
                    invocation => {
                        numbers.Add(1);
                        invocation.StopInterception();
                        return ValueTask.CompletedTask;
                    }))
                .AddInterceptor(new JSCallingBackFunctionalObjectInterceptor(
                    invocation => {
                        numbers.Clear();
                        return ValueTask.CompletedTask;
                    }))
                .BuildInterceptableFunctionalObject();

            // Act
            await jsFunctionalObject.InvokeVoidAsync();

            // Assert
            Assert.Equal(new[] { 0, 1 }, numbers);
        }
    }
}
