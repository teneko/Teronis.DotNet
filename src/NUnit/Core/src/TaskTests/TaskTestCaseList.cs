// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// A list of many <see cref="SlimLazy{T}"/> whose generic
    /// type is <see cref="Task"/>.
    /// </summary>
    public class TaskTestCaseList : Collection<TaskTestCase>
    {
        public async Task AwaitEachTaskButIgnoreExceptionsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var inlineLazy in Items) {
                try {
                    inlineLazy.SetValueIfNotCreated(cancellationToken);
                    await inlineLazy.Value;
                } catch {
                    ; // Ignore intentionally.
                }
            }
        }

        public void AddRange(IEnumerable<TaskTestCase> testTasks) {
            foreach (var testTask in testTasks) {
                Add(testTask);
            }
        }
    }
}
