using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public abstract class TaskTests<TDerived> : ITaskTests, IInitializableTaskTests
        where TDerived : TaskTests<TDerived>
    {
        private static LazyTaskList temporaryAwaitableTestList = new LazyTaskList();

        protected virtual LazyTaskList awaitableTestList { get; }

        protected TaskTests(LazyTaskList? awaitableTestList) =>
            this.awaitableTestList = awaitableTestList ?? new LazyTaskList();

        protected TaskTests() =>
            awaitableTestList = new LazyTaskList();

        public static LazyTask AddTest(LazyTaskList taskTestList, Func<Task> func, [CallerMemberName] string? callerName = null)
        {
            var inlineLazy = new LazyTask(new Func<Task>(async () => {
                try {
                    await func.Invoke();
                } catch (Exception error) {
                    throw new Exception($"Test \"{ callerName }\" failed", error);
                }
            }));

            taskTestList.Add(inlineLazy);
            return inlineLazy;
        }

        public static LazyTask AddTest(Func<Task> testProvider, [CallerMemberName] string? callerName = null) =>
            AddTest(temporaryAwaitableTestList, testProvider, callerName: callerName);

        protected virtual void InitializeInstance()
        {
            awaitableTestList.AddRange(temporaryAwaitableTestList);
            temporaryAwaitableTestList.Clear();
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

        public Task AwaitTaskTestsWhileIgnoringExceptionsAsync() =>
            awaitableTestList.AwaitEachTaskWhileIgnoringExceptionsAsync();

        /// <summary>
        /// Returns all tasks that you can await (e.g. rethrow (assertion) exceptions).
        /// </summary>
        /// <returns>The tasks (yielded).</returns>
        public IEnumerable<Task> GetAwaitableTasksToBeTested()
        {
            foreach (var lazy in awaitableTestList) {
                yield return lazy.Value;
            }
        }

        void IInitializableTaskTests.Initialize() =>
            InitializeInstance();
    }
}
