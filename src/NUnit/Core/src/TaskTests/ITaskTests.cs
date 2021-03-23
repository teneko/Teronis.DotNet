using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public interface ITaskTests
    {
        IEnumerable<Task> GetAwaitableTasksToBeTested();
        Task AwaitTaskTestsWhileIgnoringExceptionsAsync();
    }
}
