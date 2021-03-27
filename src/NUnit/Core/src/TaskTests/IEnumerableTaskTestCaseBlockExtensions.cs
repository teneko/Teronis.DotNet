// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

namespace Teronis.NUnit.TaskTests
{
    public static class IEnumerableTaskTestCaseBlockExtensions
    {
        /// <summary>
        /// See <see cref="TaskTestCaseBlockPreparer.PrepareTasksAssertion(IEnumerable{ITaskTestCaseBlock}, CancellationToken)"/>.
        /// </summary>
        /// <param name="taskTestCaseBlocks"></param>
        /// <param name="testBlockPreparer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<TaskTestCaseParameters> PrepareTasksAssertion(
            this IEnumerable<ITaskTestCaseBlock> taskTestCaseBlocks,
            TaskTestCaseBlockPreparer testBlockPreparer,
            CancellationToken cancellationToken = default) =>
            testBlockPreparer.PrepareTasksAssertion(taskTestCaseBlocks, cancellationToken: cancellationToken);
    }
}
