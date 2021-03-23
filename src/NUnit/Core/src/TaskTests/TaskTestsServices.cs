using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// This class is designed to prepare classes that are registered as <see cref="ITaskTests"/> service.
    /// </summary>
    public class TaskTestsServices
    {
        public IServiceProvider ServiceProvider { get; }

        public TaskTestsServices(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IEnumerable<ITaskTests> GetServices() =>
            ((IEnumerable<ITaskTests>)ServiceProvider.GetService(typeof(IEnumerable<ITaskTests>))).ToList();

        /// <summary>
        /// Awaits all assertable tasks in each <see cref="ITaskTests"/> instance
        /// by calling <see cref="ITaskTests.PrepareTasksAssertion"/>.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns>
        /// Represents the list of finished but assertable tasks. Store it in static
        /// scope and use it in combination with NUnit's TestCaseSource attribute.
        /// </returns>
        public async Task<IEnumerable<Task>> PrepareTasksAssertion(IEnumerable<ITaskTests> instances)
        {
            foreach (var instance in instances) {
                await instance.PrepareTasksAssertion();
            }

            return instances.SelectMany(x => x.GetAssertableTasks());
        }

        /// <summary>
        /// Awaits all assertable tasks in each <see cref="ITaskTests"/> instance
        /// by calling <see cref="ITaskTests.PrepareTasksAssertion"/>.
        /// </summary>
        /// <returns>
        /// Represents the list of finished but assertable tasks. Store it in static
        /// scope and use it in combination with NUnit's TestCaseSource attribute.
        /// </returns>
        public async Task<IEnumerable<Task>> PrepareTasksAssertion()
        {
            var instances = GetServices();
            return await PrepareTasksAssertion(instances);
        }
    }
}
