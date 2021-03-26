// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.NUnit.TaskTests
{
    public interface IInitializableTaskTests
    {
        /// <summary>
        /// Initializes the instance. (Compare <see cref="TaskTests{TDerived}"/>).
        /// Used only in <see cref="TaskTestsAnnotatedClasses"/> when assigning instances.
        /// </summary>
        public void Initialize();
    }
}
