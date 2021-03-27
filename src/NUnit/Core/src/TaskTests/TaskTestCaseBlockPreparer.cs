// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Prepare instances of type <see cref="ITaskTestCaseBlock"/>. The results of 
    /// <see cref="PrepareTasksAssertion(IEnumerable{ITaskTestCaseBlock}, CancellationToken)"/>
    /// you can use with <see cref="AsyncEnumerable.ToListAsync{TSource}(IAsyncEnumerable{TSource}, CancellationToken)"/>
    /// and that result in return you can store at example to <see cref="TaskTestCaseSourceTestFixture.TaskTestCases"/>
    /// that is used by <see cref="TaskTestCaseSourceTestFixture.AssertTask(Task)"/>.
    /// </summary>
    public class TaskTestCaseBlockPreparer : ITaskTestCaseBlockPreparer
    {
        public async IAsyncEnumerable<TaskTestCaseParameters> PrepareTasksAssertion(IEnumerable<ITaskTestCaseBlock> instances, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var instance in instances) {
                await instance.PrepareTasksAssertion(cancellationToken);

                foreach (var testCase in instance.GetTestCases()) {
                    yield return testCase.ToTestCaseParameters();
                }
            }
        }
    }
}
