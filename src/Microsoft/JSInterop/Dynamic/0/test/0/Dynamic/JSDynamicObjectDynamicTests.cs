// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.DynamicObjects;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;
using static Teronis.Utils.ICollectionGenericUtils;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectDynamicTests : JSDynamicObjectTestsBase
    {
        [Fact]
        public async Task Expect_identifier_equal_invoked_method()
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

        public async Task Assert_cancellable_object_is_cancelled_async<SecondArgumentType>(
            SecondArgumentType secondArgument,
            Func<ICancellableAnnotatedDynamicObject, Func<string, SecondArgumentType, object?, ValueTask>> getCallback)
        {
            // Arrange
            var jsObjectReference = new CancellableObjectReference();
            var jsDynamicObject = JSDynamicObjectActivator.CreateInstance<ICancellableAnnotatedDynamicObject>(jsObjectReference);

            // Assert
            var error = await Assert.ThrowsAsync<ObjectReferenceInvocationCanceledException>(async () =>
                await getCallback(jsDynamicObject)(
                    nameof(Assert_cancellable_object_is_cancelled_async),
                    secondArgument,
                    nameof(Assert_cancellable_object_is_cancelled_async)));

            Assert.Equal(
                new object[] {
                    nameof(Assert_cancellable_object_is_cancelled_async),
                    nameof(Assert_cancellable_object_is_cancelled_async)
                },
                error.JavaScriptArguments);
        }

        [Fact]
        public async Task Throw_token_cancellation_via_annotation()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token; // Get token before cancel.
            cancellationTokenSource.Cancel();

            // Assert
            await Assert_cancellable_object_is_cancelled_async(cancellationToken, jsDynamicObject => jsDynamicObject.CancelViaCancellationToken);
        }

        [Fact]
        public async Task Throw_timeout_cancellation_via_annotation()
        {
            // Arrange
            var timeout = TimeSpan.Zero;

            // Assert
            await Assert_cancellable_object_is_cancelled_async(timeout, jsDynamicObject => jsDynamicObject.CancelViaTimeSpan);
        }

        [Fact]
        public async Task Expect_accommodated_arguments()
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
        public async Task Get_identifier_via_annotation()
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
        public async Task Get_explicit_generic_constrained_ballast()
        {
            // Arrange
            var jsObjectReference = new FirstParameterReturningObjectReference();
            var jsDynamicObjectActivator = new JSDynamicProxyActivator();
            var jsDynamicObject = jsDynamicObjectActivator.CreateInstance<IExplicitGenericConstrainedDynamicObject>(jsObjectReference);

            // Act
            var expectedBallast = nameof(Get_explicit_generic_constrained_ballast);
            var resultingBallast = await jsDynamicObject.TakeAndReturnBallast(expectedBallast);

            // Assert
            Assert.Equal(expectedBallast, resultingBallast);
        }
    }
}
