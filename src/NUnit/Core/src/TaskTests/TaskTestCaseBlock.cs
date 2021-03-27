// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// This class is inteded to be inherited. In its derived class it is intended to implement a
    /// static member of such derived class. In contrast to <see cref="TaskTestCaseBlock{TDerived}"/> this
    /// class has only the manual way to initialize the static member. You should store a static
    /// instance of <see cref="NUnit.TaskTests.TaskTestCaseList"/> in derived class and pass it always as parameter
    /// to <see cref="AddTest(TaskTestCaseList, Func{CancellationToken, Task}, string?)"/>. By calling
    /// <see cref="GetTestCases"/> you get the assertable tasks, but before that you need
    /// prepare them by calling <see cref="PrepareTasksAssertion"/>.
    /// </summary>
    public abstract class TaskTestCaseBlock : ITaskTestCaseBlock, IInitializableTaskTestCaseBlock
    {
        internal static TaskTestCaseList TemporaryTaskTestCaseList = new TaskTestCaseList();

        protected virtual TaskTestCaseList TaskTestCaseList { get; }

        protected TaskTestCaseBlock(TaskTestCaseList assertableTaskFactoryList) =>
            TaskTestCaseList = assertableTaskFactoryList ?? throw new ArgumentNullException(nameof(assertableTaskFactoryList));

        /// <summary>
        /// Adds <paramref name="assertableTaskFactory"/> to <paramref name="assertableTaskFactoryList"/>.
        /// </summary>
        /// <param name="assertableTaskFactoryList"></param>
        /// <param name="assertableTaskFactory">The assertable task factory.</param>
        /// <param name="testName">Inferred by <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns></returns>
        public static TaskTestCase AddTest(
            TaskTestCaseList assertableTaskFactoryList,
            Func<CancellationToken, Task> assertableTaskFactory,
            [CallerMemberName] string? testName = null)
        {
            var testCaseData = new TestCaseData()
                .SetName(testName);

            var taskTestCase = new TaskTestCase(assertableTaskFactory, testCaseData);
            assertableTaskFactoryList.Add(taskTestCase);
            return taskTestCase;
        }

        /// <summary>
        /// Extends the initializing procedure when calling <see cref="IInitializableTaskTestCaseBlock.Initialize"/>.
        /// </summary>
        protected virtual void InitializeInstance()
        {
            TaskTestCaseList.AddRange(TemporaryTaskTestCaseList);
            TemporaryTaskTestCaseList.Clear();
        }

        public virtual Task PrepareTasksAssertion(CancellationToken cancellationToken = default) =>
            TaskTestCaseList.AwaitEachTaskButIgnoreExceptionsAsync(cancellationToken);

        /// <summary>
        /// Returns all tasks that you can await (e.g. rethrow (assertion) exceptions)
        /// from <see cref="TaskTestCaseList"/>. Each assertable task represents
        /// one test.
        /// </summary>
        /// <returns>The test cases (yielded).</returns>
        public IEnumerable<TaskTestCase> GetTestCases()
        {
            foreach (var testCase in TaskTestCaseList) {
                yield return testCase;
            }
        }

        void IInitializableTaskTestCaseBlock.Initialize() =>
            InitializeInstance();
    }
}
