// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Dynamic.DynamicObjects;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectActivationTests : JSDynamicObjectTestsBase
    {
        [Fact]
        public void Throw_not_of_type_value_task_exception()
        {
            // Arrange
            var jsObjectReference = new EmptyObjectReference();

            // Act & Assert
            var error = Assert.Throws<NotSupportedException>(() => JSDynamicObjectActivator.CreateInstance<INotOfTypeValueTaskDynamicObject>(jsObjectReference));
            Assert.Equal(ValueTaskType.ThrowHelper.CreateNotOfTypeValueTaskException(typeof(string)).Message, error.Message);
        }

        [Fact]
        public void Throw_parameter_after_accommodatable_annotated_parameter_exception()
        {
            // Arrange
            var jsObjectReference = new EmptyObjectReference();

            // Act & Assert
            Assert.Throws<ParameterListException>(() => JSDynamicObjectActivator.CreateInstance<IMisuedAccommodatableAnnotatedDynamicObject>(jsObjectReference));
        }

        [Fact]
        public void Throw_too_many_cancellable_annotated_parameter_exception()
        {
            // Arrange
            var jsObjectReference = new EmptyObjectReference();

            // Act & Assert
            Assert.Throws<ParameterListException>(() => JSDynamicObjectActivator.CreateInstance<IMisuedCancellableAnnotatedDynamicObject>(jsObjectReference));
        }
    }
}
