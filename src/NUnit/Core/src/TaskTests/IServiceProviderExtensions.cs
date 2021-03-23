using System;

namespace Teronis.NUnit.TaskTests
{
    public static class IServiceProviderExtensions
    {
        public static TaskTestsServices ToTaskTestsServices(this IServiceProvider serviceProvider) =>
            new TaskTestsServices(serviceProvider);

        public static TaskTestsAnnotatedClasses ToTasksTestsAnnotatedClasses(this IServiceProvider serviceProvider) =>
            new TaskTestsAnnotatedClasses(serviceProvider);
    }
}
