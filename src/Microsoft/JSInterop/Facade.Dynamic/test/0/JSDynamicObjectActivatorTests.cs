using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.JSObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public class JSDynamicObjectActivatorTests
    {
        [Fact]
        public async Task Should_dispose_object_reference_through_dynamic_object() {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            await emptyDynamicObject.DisposeAsync();

            // Assert
            Assert.True(jsObjectReference.IsDisposed);
        }

        [Fact]
        public async Task Should_expect_identifier_as_passed()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var emptyDynamicObject = JSDynamicObjectActivator.CreateInstance<IEmptyDynamicObject>(jsObjectReference);

            // Act
            var expectedIdentifier = nameof(Should_expect_identifier_as_passed);
            // The implemented interface member calls get precedence over dynamic object method calls.
            var resultedIdentifier = await emptyDynamicObject.InvokeAsync<string>(expectedIdentifier);

            // Assert
            Assert.Equal(expectedIdentifier, resultedIdentifier);
        }

        [Fact]
        public void Should_expect_return_type_not_of_type_value_task_exception()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();

            // Act & Assert
            var error = Assert.Throws<NotSupportedException>(() => JSDynamicObjectActivator.CreateInstance<INotOfTypeValueTaskDynamicObject>(jsObjectReference));
            Assert.Equal(ValueTaskType.ThrowHelper.CreateNotOfTypeValueTaskException(typeof(string)).Message, error.Message);
        }

        [Fact]
        public async Task Should_expect_identifier_as_invoked()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<IIdentifierReceivableDynamicObject>(jsObjectReference);

            // Act
            var expectedIdentifier = nameof(IIdentifierReceivableDynamicObject.ReceiveIdentifier);
            var resultedIdentifier = await jsDynamicObject.ReceiveIdentifier();

            // Assert
            Assert.Equal(expectedIdentifier, resultedIdentifier);
        }

        [Fact]
        public async Task Should_expect_cancellation_through_cancellation_token()
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
                //await jsDynamicObject.InvokeAsync<string>(nameof(Should_expect_cancellation_through_cancellation_token), cancellationToken, args: null));
                await jsDynamicObject.InvokeAsync<string>(nameof(Should_expect_cancellation_through_cancellation_token), cancellationToken));
        }

        public async Task AssertCancellableObjectIsCancelledAsync<SecondArgumentType>(
            SecondArgumentType secondArgument,
            Func<ICancellableAnnotatedMethodObjectProxy, Func<string, SecondArgumentType, object?, ValueTask>> getCallback)
        {
            // Arrange
            var jsObjectReference = new CancellableObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<ICancellableAnnotatedMethodObjectProxy>(jsObjectReference);

            // Assert
            var error = await Assert.ThrowsAsync<ObjectReferenceInvocationCanceledException>(async () =>
                await getCallback(jsDynamicObject)(
                    nameof(AssertCancellableObjectIsCancelledAsync),
                    secondArgument,
                    nameof(AssertCancellableObjectIsCancelledAsync)));

            Assert.Equal(
                new object[] {
                    nameof(AssertCancellableObjectIsCancelledAsync),
                    nameof(AssertCancellableObjectIsCancelledAsync)
                },
                error.JavaScriptArguments);
        }

        [Fact]
        public async Task Should_expect_cancellation_through_cancellation_token_via_annotation()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token; // Get token before cancel.
            cancellationTokenSource.Cancel();

            // Assert
            await AssertCancellableObjectIsCancelledAsync(cancellationToken, jsDynamicObject => jsDynamicObject.CancelViaCancellationToken);
        }

        [Fact]
        public async Task Should_expect_cancellation_through_time_span_via_annotation()
        {
            // Arrange
            var timeout = TimeSpan.Zero;

            // Assert
            await AssertCancellableObjectIsCancelledAsync(timeout, jsDynamicObject => jsDynamicObject.CancelViaTimeSpan);
        }
    }
}
