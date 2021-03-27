// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public interface ITaskTestCaseBlock
    {
        /// <summary>
        /// Gets the list of assertable tasks.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskTestCase> GetTestCases();
        /// <summary>
        /// Awaits each assertable tasks while ignoring occuring exceptions.
        /// </summary>
        /// <returns>Represents the operation.</returns>
        Task PrepareTasksAssertion(CancellationToken cancellationToken = default);
    }
}
