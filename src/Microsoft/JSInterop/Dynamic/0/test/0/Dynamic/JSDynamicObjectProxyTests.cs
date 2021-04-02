// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.DynamicObjects;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectProxyTests : JSDynamicObjectTestsBase
    {
        [Fact]
        public async Task Should_dispose()
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
        public async Task Expect_equal_input_output()
        {
            // Arrange
            var jsObjectReference = new JSArgumentsPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            var expectedContent = nameof(Expect_equal_input_output);
            // The extension methods get precedence over target method calls.
            var resultedArguments = await emptyDynamicObject.InvokeAsync<object[]>(identifier: string.Empty, expectedContent);

            // Assert
            Assert.Equal(new object[] { expectedContent }, resultedArguments);
        }

        [Fact]
        public void Get_property()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            Assert.True(ReferenceEquals(jsObjectReference, emptyDynamicObject.ObjectReference));
        }

        [Fact]
        public async Task Throw_token_cancellation()
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
                await jsDynamicObject.InvokeAsync<string>(nameof(Throw_token_cancellation), cancellationToken));
        }
    }
}
