using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// This class is designed to prepare classes that are annotated with <see cref="TaskTestsAttribute"/>.
    /// </summary>
    public class TaskTestsAnnotatedClasses
    {
        public IServiceProvider ServiceProvider { get; }

        public TaskTestsAnnotatedClasses(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private List<TaskTestsAnnotatedClassCollectorEntry> CollectTaskTestsClassInstance(Assembly assembly)
        {
            var taskTestsClassInstanceCollector = new TaskTestsAnnotatedClassCollector(assembly);
            var taskTestsClassInstances = taskTestsClassInstanceCollector.Collect().ToList();
            return taskTestsClassInstances;
        }

        /// <summary>
        /// Sets all instances that are null via <see cref="ActivatorUtilities"/>.
        /// If an activated instance is of type <see cref="TaskTests{TDerived}"/>
        /// the <see cref="TaskTests{TDerived}.Initialize"/> method will be called.
        /// </summary>
        /// <param name="collectorEntries"></param>
        public void AssignInstances(List<TaskTestsAnnotatedClassCollectorEntry> collectorEntries)
        {
            foreach (var collectorEntry in collectorEntries) {
                if (collectorEntry.Instance is null) {
                    var instanceType = collectorEntry.GetInstanceType();
                    var instance = (ITaskTests)ActivatorUtilities.CreateInstance(ServiceProvider, instanceType);

                    if (instance is IInitializableTaskTests initializableInstance) {
                        initializableInstance.Initialize();
                    }

                    collectorEntry.SetInstance(instance);
                }
            }
        }

        /// <summary>
        /// Collects all classes via <see cref="TaskTestsAnnotatedClassCollector"/> 
        /// and sets all instances that are null via <see cref="ActivatorUtilities"/>.
        /// If an activated instance is of type <see cref="TaskTests{TDerived}"/>
        /// the <see cref="TaskTests{TDerived}.Initialize"/> method will be called.
        /// </summary>
        public void AssignInstances(Assembly assembly)
        {
            var collectorEntries = CollectTaskTestsClassInstance(assembly);
            AssignInstances(collectorEntries);
        }

        /// <summary>
        /// Awaits all assertable tasks in each <see cref="ITaskTests"/> instance
        /// by calling <see cref="ITaskTests.PrepareTasksAssertion"/>.
        /// </summary>
        /// <param name="collectorEntries"></param>
        /// <returns>
        /// Represents the list of finished but assertable tasks. Store it in static
        /// scope and use it in combination with NUnit's TestCaseSource attribute.
        /// </returns>
        public async Task<IEnumerable<Task>> PrepareTasksAssertion(List<TaskTestsAnnotatedClassCollectorEntry> collectorEntries)
        {
            foreach (var collectorEntry in collectorEntries) {
                var instance = collectorEntry.Instance ?? throw new InvalidOperationException();
                await instance.PrepareTasksAssertion();
            }

            return collectorEntries
                .Select(x => x.Instance ?? throw new InvalidOperationException("Instance was null contrary to expectations."))
                .SelectMany(x => x.GetAssertableTasks());
        }

        /// <summary>
        /// <para>
        /// Collects all classes via <see cref="TaskTestsAnnotatedClassCollector"/> 
        /// and sets all instances that are null via <see cref="ActivatorUtilities"/>.
        /// If an activated instance is of type <see cref="TaskTests{TDerived}"/>
        /// the <see cref="TaskTests{TDerived}.Initialize"/> method will be called.
        /// </para>
        /// <para>
        /// Awaits all assertable tasks in each <see cref="ITaskTests"/> instance
        /// by calling <see cref="ITaskTests.PrepareTasksAssertion"/>.
        /// </para>
        /// </summary>
        /// <param name="webAssemblyHost"></param>
        /// <returns>
        /// Represents the list of finished but assertable tasks. Store it in static
        /// scope and use it in combination with NUnit's TestCaseSource attribute.
        /// </returns>
        public async Task<IEnumerable<Task>> AssignInstancesAndPrepareTasksAssertion(Assembly assembly)
        {
            var collectorEntries = CollectTaskTestsClassInstance(assembly);
            AssignInstances(collectorEntries);
            return await PrepareTasksAssertion(collectorEntries);
        }
    }
}
