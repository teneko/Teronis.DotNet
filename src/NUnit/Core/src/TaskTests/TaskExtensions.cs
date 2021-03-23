using System.Threading.Tasks;
using NUnit.Framework;

namespace Teronis.NUnit.TaskTests
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Asserts that task is not erroneous, otherwise the inner exception is thrown.
        /// </summary>
        /// <param name="task"></param>
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
