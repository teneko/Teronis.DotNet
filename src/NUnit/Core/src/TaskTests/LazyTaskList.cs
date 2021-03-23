using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop;

namespace Teronis.NUnit.TaskTests
{
    public class LazyTaskList : Collection<SlimLazy<Task>>
    {
        public async Task AwaitEachTaskButIgnoreExceptionsAsync()
        {
            foreach (var inlineLazy in Items) {
                try {
                    await inlineLazy.Value;
                } catch {
                    // Ignore intentionally.
                }
            }
        }

        public void AddRange(IEnumerable<SlimLazy<Task>> testTasks) {
            foreach (var testTask in testTasks) {
                Add(testTask);
            }
        }
    }
}
