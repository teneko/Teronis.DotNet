// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects;
using Teronis.Microsoft.JSInterop.JSObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectProxyTests : JSDynamicObjectTestsBase
    {
        [Fact]
        public async Task Proxy_Dispose()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            await emptyDynamicObject.DisposeAsync();

            // Assert
            Assert.True(jsObjectReference.IsDisposed);
        }

        [Fact]
        public async Task Proxy_Expect_equal_input_output()
        {
            // Arrange
            var jsObjectReference = new JSArgumentsPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            var expectedContent = nameof(Proxy_Expect_equal_input_output);
            // The extension methods get precedence over target method calls.
            var resultedArguments = await emptyDynamicObject.InvokeAsync<object[]>(identifier: string.Empty, expectedContent);

            // Assert
            Assert.Equal(new object[] { expectedContent }, resultedArguments);
        }

        [Fact]
        public void Proxy_Get_property()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            Assert.True(ReferenceEquals(jsObjectReference, emptyDynamicObject.JSObjectReference));
        }

        [Fact]
        public async Task Proxy_Throw_token_cancellation()
        {
            // Arrange
            var jsObjectReference = new CancellableObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token; // Get token before cancel.
            cancellationTokenSource.Cancel();

            // Assert
            await Assert.ThrowsAsync<ObjectReferenceInvocationCanceledException>(async () =>
                await jsDynamicObject.InvokeAsync<string>(nameof(Proxy_Throw_token_cancellation), cancellationToken));
        }
    }
}
