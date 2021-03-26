// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Teronis.NUnit.TaskTests;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    public class NUnitEntryPoint
    {
        // Is initialized in index page.
        public static IEnumerable<Task> AssertableTasks = null!;

        [TestCaseSource(nameof(AssertableTasks))]
        public void ProcessTaskTests(Task assertableTask) =>
            assertableTask.AssertIsNotErroneous();
    }
}
