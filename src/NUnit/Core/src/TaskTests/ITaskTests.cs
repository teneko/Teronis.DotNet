// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public interface ITaskTests
    {
        /// <summary>
        /// Gets the list of assertable tasks.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Task> GetAssertableTasks();
        /// <summary>
        /// Awaits each assertable tasks while ignoring occuring exceptions.
        /// </summary>
        /// <returns>Represents the operation.</returns>
        Task PrepareTasksAssertion();
    }
}
