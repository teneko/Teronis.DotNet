using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.NUnit.TaskTests;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    public class NUnitEntryPoint
    {
        public static IEnumerable<Task> AssertableTasks = null!;

        [TaskTestSource(typeof(JSFacadesTests), nameof(JSFacadesTests.Instance))]
        public void ProcessTaskTests(Task assertableTask) =>
            assertableTask.AssertIsNotErroneous();
    }
}
