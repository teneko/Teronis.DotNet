using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;
using Teronis.Microsoft.JSInterop.JSObjectReferences;
using Xunit;
using static Teronis.Utils.ICollectionGenericUtils;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectTests
    {
        public JSDynamicProxyActivator JSDynamicObjectActivator { get; }

        public JSDynamicObjectTests() =>
            JSDynamicObjectActivator = new JSDynamicProxyActivator();

        [Fact]
        public void Activation_Throw_not_of_type_value_task_exception()
        {
            // Arrange
            var jsObjectReference = new JSEmptyObjectReference();

            // Act & Assert
            var error = Assert.Throws<NotSupportedException>(() => JSDynamicObjectActivator.CreateInstance<INotOfTypeValueTaskDynamicObject>(jsObjectReference));
            Assert.Equal(ValueTaskType.ThrowHelper.CreateNotOfTypeValueTaskException(typeof(string)).Message, error.Message);
        }

        [Fact]
        public void Activation_Throw_parameter_after_accommodatable_annotated_parameter_exception()
        {
            // Arrange
            var jsObjectReference = new JSEmptyObjectReference();

            // Act & Assert
            Assert.Throws<ParameterListException>(() => JSDynamicObjectActivator.CreateInstance<IMisuedAccommodatableAnnotatedDynamicObject>(jsObjectReference));
        }

        [Fact]
        public void Activation_Throw_too_many_cancellable_annotated_parameter_exception()
        {
            // Arrange
            var jsObjectReference = new JSEmptyObjectReference();

            // Act & Assert
            Assert.Throws<ParameterListException>(() => JSDynamicObjectActivator.CreateInstance<IMisuedCancellableAnnotatedDynamicObject>(jsObjectReference));
        }

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

        [Fact]
        public async Task Dynamic_Expect_identifier_equal_invoked_method()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<IIdentifierPromisingDynamicObject>(jsObjectReference);

            // Act
            var expectedIdentifier = nameof(IIdentifierPromisingDynamicObject.GetIdentifier);
            var resultedIdentifier = await jsDynamicObject.GetIdentifier();

            // Assert
            Assert.Equal(expectedIdentifier, resultedIdentifier);
        }

        public async Task AssertCancellableObjectIsCancelledAsync<SecondArgumentType>(
            SecondArgumentType secondArgument,
            Func<ICancellableAnnotatedDynamicObject, Func<string, SecondArgumentType, object?, ValueTask>> getCallback)
        {
            // Arrange
            var jsObjectReference = new CancellableObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<ICancellableAnnotatedDynamicObject>(jsObjectReference);

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
        public async Task Dynamic_Throw_token_cancellation_via_annotation()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token; // Get token before cancel.
            cancellationTokenSource.Cancel();

            // Assert
            await AssertCancellableObjectIsCancelledAsync(cancellationToken, jsDynamicObject => jsDynamicObject.CancelViaCancellationToken);
        }

        [Fact]
        public async Task Dynamic_Throw_timeout_cancellation_via_annotation()
        {
            // Arrange
            var timeout = TimeSpan.Zero;

            // Assert
            await AssertCancellableObjectIsCancelledAsync(timeout, jsDynamicObject => jsDynamicObject.CancelViaTimeSpan);
        }

        [Fact]
        public async Task Dynamic_Expect_accommodated_arguments()
        {
            // Arrange
            var jsObjectReference = new JSArgumentsPromisingObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<IAccommodatableAnnotatedDynamicObject>(jsObjectReference);
            var ballast = new object();
            var accommodatedArguments = new object?[] { "test", 2 };

            // Act
            var jsArguments = await jsDynamicObject.GetJavaScriptArguments(ballast, CancellationToken.None, accommodatedArguments);

            // Assert
            Assert.Equal(
                expected: AddItemRangeAndReturnList(new List<object?>() { { ballast } }, accommodatedArguments),
                actual: jsArguments); // Checks sequence equality.
        }

        [Fact]
        public async Task Dynamic_Get_identifier_via_annotation()
        {
            // Arrange
            var jsObjectReference = new IdentifierPromisingObjectReference();
            var jsDynamicObjectActivator = new JSDynamicProxyActivator();
            var jsDynamicObject = jsDynamicObjectActivator.CreateInstance<IIdentifierAnnotatedDynamicObject>(jsObjectReference);

            // Act
            var javaScriptIdentifier = await jsDynamicObject.CSharpTypicalMethodNameAsync();

            // Assert
            Assert.Equal(IIdentifierAnnotatedDynamicObject.javaScriptTypicalMethodName, javaScriptIdentifier);
        }

        // Redundant test because IJSObjectReferenceFacade has already generic constrained method definitions.
        [Fact]
        public async Task Dynamic_Get_explicit_generic_constrained_ballast()
        {
            // Arrange
            var jsObjectReference = new FirstParameterReturningObjectReference();
            var jsDynamicObjectActivator = new JSDynamicProxyActivator();
            var jsDynamicObject = jsDynamicObjectActivator.CreateInstance<IExplicitGenericConstrainedDynamicObject>(jsObjectReference);

            // Act
            var expectedBallast = nameof(Dynamic_Get_explicit_generic_constrained_ballast);
            var resultingBallast = await jsDynamicObject.TakeAndReturnBallast(expectedBallast);

            // Assert
            Assert.Equal(expectedBallast, resultingBallast);
        }
    }
}
