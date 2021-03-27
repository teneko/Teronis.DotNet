// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Prepare instances of type <see cref="ITaskTestCaseBlock"/>.
    /// </summary>
    public interface ITaskTestCaseBlockPreparer
    {
        /// <summary>
        /// Awaits all assertable tasks in each <see cref="ITaskTestCaseBlock"/> instance
        /// by calling <see cref="ITaskTestCaseBlock.PrepareTasksAssertion"/>.
        /// </summary>
        /// <param name="instances">Instances of type <see cref="ITaskTestCaseBlock"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Represents the list of assertable tasks that are about to finish. Store it in static
        /// scope and use it in combination with <see cref="TestCaseSourceAttribute"/>
        /// and <see cref="TestCaseSourceAttribute.SourceName"/>.
        /// </returns>
        IAsyncEnumerable<TaskTestCaseParameters> PrepareTasksAssertion(IEnumerable<ITaskTestCaseBlock> instances, CancellationToken cancellationToken = default);
    }
}
