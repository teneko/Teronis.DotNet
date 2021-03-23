using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop;

namespace Teronis.NUnit.TaskTests
{
    public class LazyTask : SlimLazy<Task>
    {
        public LazyTask(Func<Task> taskProvider)
            : base(taskProvider) { }
    }
}
