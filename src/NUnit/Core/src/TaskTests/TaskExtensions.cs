using System.Threading.Tasks;
using NUnit.Framework;

namespace Teronis.NUnit.TaskTests
{
    public static class TaskExtensions
    {
        /// <summary>
        /// <para>
        /// Asserts that task is not erroneous, otherwise the
        /// inner exception is thrown.
        /// </para>
        /// <para>
        /// You may call it in a
        /// test case that is decorated with 
        /// <see cref="TaskTestsSourceAttribute"/> or
        /// <see cref="TestCaseSourceAttribute"/> that provides 
        /// assertable task as first parameter.
        /// </para>
        /// </summary>
        /// <param name="task">The task to be asserted.</param>
        public static void AssertIsNotErroneous(this Task task)
        {
            if (task.Exception is null) {
                Assert.Pass();
            }

            Assert.Multiple(() => {
                foreach (var error in task.Exception!.InnerExceptions) {
                    Assert.DoesNotThrow(() => throw error);
                }
            });
        }
    }
}
