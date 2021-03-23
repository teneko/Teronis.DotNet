using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.NUnit.TaskTests;

namespace Teronis.AspNetCore.Components.NUnit
{
    public static class WebAssemblyHostExtensions
    {
        private static List<TaskTestsClassInstanceCollectorEntry> CollectTaskTestsClassInstance(Assembly assembly)
        {
            var taskTestsClassInstanceCollector = new TaskTestsClassInstanceCollector(assembly);
            var taskTestsClassInstances = taskTestsClassInstanceCollector.Collect().ToList();
            return taskTestsClassInstances;
        }

        private static void AssignTaskTestsClassInstances(List<TaskTestsClassInstanceCollectorEntry> collectorEntries, IServiceProvider serviceProvider)
        {
            foreach (var collectorEntry in collectorEntries) {
                if (collectorEntry.Instance is null) {
                    var instanceType = collectorEntry.GetInstanceType();
                    var instance = (ITaskTests)ActivatorUtilities.CreateInstance(serviceProvider, instanceType);

                    if (instance is IInitializableTaskTests initializableInstance) {
                        initializableInstance.Initialize();
                    }

                    collectorEntry.SetInstance(instance);
                }
            }
        }

        private static async Task AwaitTaskTestsWhileIgnoringExceptionsAsync(List<TaskTestsClassInstanceCollectorEntry> collectorEntries)
        {
            foreach (var collectorEntry in collectorEntries) {
                var instance = collectorEntry.Instance ?? throw new InvalidOperationException();
                await instance.AwaitTaskTestsWhileIgnoringExceptionsAsync();
            }
        }

        /// <summary>
        /// Collects all classes via <see cref="TaskTestsClassInstanceCollector"/> 
        /// and sets all instances that are null via <see cref="ActivatorUtilities"/>.
        /// If an activated instance is of type <see cref="TaskTests{TDerived}"/>
        /// the <see cref="TaskTests{TDerived}.Initialize"/> method will be called.
        /// </summary>
        /// <param name="webAssemblyHost"></param>
        /// <returns><paramref name="webAssemblyHost"/></returns>
        public static WebAssemblyHost AssignTaskTestsClassInstances(this WebAssemblyHost webAssemblyHost, Assembly assembly)
        {
            var serviceProvider = webAssemblyHost.Services;
            var collectorEntries = CollectTaskTestsClassInstance(assembly);
            AssignTaskTestsClassInstances(collectorEntries, serviceProvider);
            return webAssemblyHost;
        }

        /// <summary>
        /// <para>
        /// Collects all classes via <see cref="TaskTestsClassInstanceCollector"/> 
        /// and sets all instances that are null via <see cref="ActivatorUtilities"/>.
        /// If an activated instance is of type <see cref="TaskTests{TDerived}"/>
        /// the <see cref="TaskTests{TDerived}.Initialize"/> method will be called.
        /// </para>
        /// <para>
        /// Awaits all task tests in each task tests instance by calling
        /// <see cref="ITaskTests.AwaitTaskTestsWhileIgnoringExceptionsAsync"/>.
        /// </para>
        /// </summary>
        /// <param name="webAssemblyHost"></param>
        /// <returns>
        /// Represents the list of finished but assertable tasks. Store it in static
        /// scope and use it in combination with NUnit's TestCaseSource attribute.
        /// </returns>
        public static async Task<IEnumerable<Task>> AssignAndInitTaskTestsClassInstancesAsync(this WebAssemblyHost webAssemblyHost, Assembly assembly)
        {
            var serviceProvider = webAssemblyHost.Services;
            var collectorEntries = CollectTaskTestsClassInstance(assembly);
            AssignTaskTestsClassInstances(collectorEntries, serviceProvider);
            await AwaitTaskTestsWhileIgnoringExceptionsAsync(collectorEntries);

            return collectorEntries
                .Select(x => x.Instance ?? throw new InvalidOperationException("Instance was null contrary to expectations."))
                .SelectMany(x => x.GetAwaitableTasksToBeTested());
        }
    }
}
