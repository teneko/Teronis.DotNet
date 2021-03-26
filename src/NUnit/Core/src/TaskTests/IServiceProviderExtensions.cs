// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
