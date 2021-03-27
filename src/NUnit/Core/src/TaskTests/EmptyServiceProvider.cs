// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Represents default implementation of <see cref="IServiceProvider"/>
    /// where <see cref="IServiceProvider.GetService(Type)"/> activates the
    /// requested service by using <see cref="Activator.CreateInstance(Type)"/>.
    /// </summary>
    public sealed class EmptyServiceProvider : IServiceProvider
    {
        public static EmptyServiceProvider Instance = new EmptyServiceProvider();

        private EmptyServiceProvider() { }

        public object GetService(Type serviceType) =>
            Activator.CreateInstance(serviceType);
    }
}
