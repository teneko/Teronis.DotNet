using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public static class IServiceProviderExtensions
    {
        public static async Task<IServiceProvider> AwaitTaskTestsWhileIgnoringExceptionsAsync(this IServiceProvider serviceProvider)
        {
            var taskTestsList = (IEnumerable<ITaskTests>)serviceProvider.GetService(typeof(IEnumerable<ITaskTests>));

            foreach (var taskTests in taskTestsList) {
                await taskTests.AwaitTaskTestsWhileIgnoringExceptionsAsync();
            }

            return serviceProvider;
        }
    }
}
