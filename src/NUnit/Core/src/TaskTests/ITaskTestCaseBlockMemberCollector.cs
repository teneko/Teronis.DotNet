// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Provides a mechanism to collect <see cref="TaskTestCaseBlockMember"/>.
    /// </summary>
    public interface ITaskTestCaseBlockMemberCollector
    {
        /// <summary>
        /// Collects class members which implements <see cref="ITaskTestCaseBlock"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskTestCaseBlockMember> CollectMembers();
    }
}
