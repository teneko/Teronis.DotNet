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
    /// <para>
    /// This class is inteded to be inherited. In its derived class it is intended to implement a
    /// static member of such derived class. There are three ways to initialize the static member.
    /// </para>
    /// <para>
    /// First store a static instance of <see cref="NUnit.TaskTests.TaskTestCaseList"/> in derived
    /// class and pass it as parameter to constructor of <see cref="TaskTestCaseBlock"/>. Then use
    /// <see cref="AddTest(TaskTestCaseList, Func{CancellationToken, Task}, string?)"/>. By calling
    /// <see cref="GetTestCases"/> you get the <see cref="TaskTestCaseParameters"/>, but before that you
    /// need to prepare them by calling <see cref="PrepareTasksAssertion"/>.
    /// </para>
    /// <para>
    /// The more automatic way is to use the parameterless constructor of <see cref="TaskTestCaseBlock"/>.
    /// It passes a <see cref="TaskTestCaseList"/> to its base type <see cref="TaskTestCaseBlock"/>. Because
    /// you can't to call <see cref="AddTest(TaskTestCaseList, Func{CancellationToken,Task}, string?)"/>
    /// but <see cref="TaskTestCaseBlock.AddTest(Func{CancellationToken,Task}, string?)"/>, you have to call
    /// <see cref="Initialize"/> after the creation of derived class.
    /// </para>
    /// <para>
    /// Last but not least you can you just skip the instantiation of the static member. Then you must
    /// decorate the derived class with <see cref="TaskTestCaseBlockStaticMemberProviderAttribute"/>. After
    /// that you can leave the instantiaton to <see cref="AssemblyTaskTestCaseBlockStaticMemberCollector"/>
    /// in conjunction with <see cref="TaskTestCaseBlockMemberAssigner"/>.
    /// </para>
    /// </summary>
    public abstract class TaskTestCaseBlock : ITaskTestCaseBlock, IInitializableTaskTestCaseBlock
    {
        internal static TaskTestCaseList TemporaryTaskTestCaseList = new TaskTestCaseList();

        protected virtual TaskTestCaseList TaskTestCaseList { get; }

        protected TaskTestCaseBlock(TaskTestCaseList assertableTaskFactoryList) =>
            TaskTestCaseList = assertableTaskFactoryList ?? throw new ArgumentNullException(nameof(assertableTaskFactoryList));

        protected TaskTestCaseBlock()
            : this(new TaskTestCaseList()) { }

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
        /// <para>
        /// Adds <paramref name="assertableTaskFactory"/> to an internal static task factory list.
        /// </para>
        /// <para>
        /// You must call <see cref="Initialize"/> when creating <see cref="TaskTestCaseBlock"/>
        /// manually.
        /// This moves the items from the internal static task factory list to the instance's one.
        /// </para>
        /// </summary>
        /// <param name="assertableTaskFactory"></param>
        /// <param name="testName">Inferred by <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns></returns>
        public static TaskTestCase AddTest(Func<CancellationToken, Task> assertableTaskFactory, [CallerMemberName] string? testName = null) =>
            AddTest(TemporaryTaskTestCaseList, assertableTaskFactory, testName: testName);

        /// <summary>
        /// Extends the initializing procedure when calling <see cref="IInitializableTaskTestCaseBlock.Initialize"/>.
        /// </summary>
        protected virtual void InitializeInstance()
        {
            TaskTestCaseList.AddRange(TemporaryTaskTestCaseList);
            TemporaryTaskTestCaseList.Clear();
        }

        public void Initialize() =>
            InitializeInstance();

        /// <summary>
        /// Awaits each assertable task but ignores exceptions.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task PrepareTasksAssertion(CancellationToken cancellationToken = default) =>
            TaskTestCaseList.AwaitEachTaskButIgnoreExceptionsAsync(cancellationToken);

        /// <summary>
        /// Returns all assertable tasks that you can await (e.g. rethrow (assertion) exceptions)
        /// from <see cref="TaskTestCaseList"/>. Each assertable task represents one test.
        /// </summary>
        /// <returns>The test cases (yielded).</returns>
        public IEnumerable<TaskTestCase> GetTestCases()
        {
            foreach (var testCase in TaskTestCaseList) {
                yield return testCase;
            }
        }
    }
}
