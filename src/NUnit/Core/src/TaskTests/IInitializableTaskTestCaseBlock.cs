// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.NUnit.TaskTests
{
    public interface IInitializableTaskTestCaseBlock
    {
        /// <summary>
        /// Initializes the instance. (Compare <see cref="TaskTestCaseBlock{TDerived}"/>).
        /// Used only in <see cref="TaskTestCaseBlockMemberAssigner"/> when assigning instance.
        /// </summary>
        public void Initialize();
    }
}
