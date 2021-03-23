using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public abstract class TaskTests<TDerived> : ITaskTests, IInitializableTaskTests
        where TDerived : TaskTests<TDerived>
    {
        private static LazyTaskList temporaryAssertableTaskFactoryList = new LazyTaskList();

        protected virtual LazyTaskList AssertableTaskFactoryList { get; }

        protected TaskTests(LazyTaskList? assertableTaskFactoryList) =>
            AssertableTaskFactoryList = assertableTaskFactoryList ?? new LazyTaskList();

        protected TaskTests() =>
            AssertableTaskFactoryList = new LazyTaskList();

        /// <summary>
        /// Adds <paramref name="assertableTaskFactory"/> to <paramref name="assertableTaskFactoryList"/>.
        /// </summary>
        /// <param name="assertableTaskFactoryList"></param>
        /// <param name="assertableTaskFactory">The assertable task factory.</param>
        /// <param name="testName">Inferred by <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns></returns>
        public static LazyTask AddTest(LazyTaskList assertableTaskFactoryList, Func<Task> assertableTaskFactory, [CallerMemberName] string? testName = null)
        {
            var inlineLazy = new LazyTask(new Func<Task>(async () => {
                try {
                    await assertableTaskFactory.Invoke();
                } catch (Exception error) {
                    throw new Exception($"Test \"{ testName }\" has thrown an exception.", error);
                }
            }));

            assertableTaskFactoryList.Add(inlineLazy);
            return inlineLazy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assertableTaskFactory"></param>
        /// <param name="testName">Inferred by <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns></returns>
        public static LazyTask AddTest(Func<Task> assertableTaskFactory, [CallerMemberName] string? testName = null) =>
            AddTest(temporaryAssertableTaskFactoryList, assertableTaskFactory, testName: testName);

        protected virtual void InitializeInstance()
        {
            AssertableTaskFactoryList.AddRange(temporaryAssertableTaskFactoryList);
            temporaryAssertableTaskFactoryList.Clear();
        }

        /// <summary>
        /// Copies temporary, static and to be tested lazy tasks over to instance scoped 
        /// list. Must be called when you use <see cref="AddTest(Func{Task}, string?)"/>.
        /// </summary>
        /// <returns></returns>
        public TDerived Initialize()
        {
            InitializeInstance();
            return (TDerived)this;
        }

        public virtual Task PrepareTasksAssertion() =>
            AssertableTaskFactoryList.AwaitEachTaskButIgnoreExceptionsAsync();

        /// <summary>
        /// Returns all tasks that you can await (e.g. rethrow (assertion) exceptions)
        /// from <see cref="AssertableTaskFactoryList"/>. Each assertable task represents
        /// one test.
        /// </summary>
        /// <returns>The assertable tasks (yielded).</returns>
        public IEnumerable<Task> GetAssertableTasks()
        {
            foreach (var lazy in AssertableTaskFactoryList) {
                yield return lazy.Value;
            }
        }

        void IInitializableTaskTests.Initialize() =>
            InitializeInstance();
    }
}
