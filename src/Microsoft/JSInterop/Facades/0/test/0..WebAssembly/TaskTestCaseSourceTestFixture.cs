// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Teronis.NUnit.Api;
using Teronis.NUnit.TaskTests;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    /// <summary>
    /// A NUnit test fixture for tasks.
    /// The only implemented method <see cref="AssertTask(Task)"/> is
    /// annoated with <see cref="TestCaseSourceAttribute"/> whose property
    /// <see cref="TestCaseSourceAttribute.SourceName"/> points to
    /// <see cref="TaskTestCases"/>. If this (derived) class is target of
    /// for example <see cref="NUnitSingleThreadAssemblyRunner"/> you should
    /// initialize <see cref="TaskTestCases"/>.
    /// </summary>
    public class TaskTestCaseSourceTestFixture2
    {
        /// <summary>
        /// If this (derived) class is target of
        /// for example <see cref="NUnitSingleThreadAssemblyRunner"/>
        /// you should initialize <see cref="TaskTestCases"/>.
        /// </summary>
        public static IEnumerable<TestCaseParameters>? TaskTestCases = null!;

        //[TestCaseSource(nameof(TaskTestCases))]
        public void AssertTask(Task assertableTask) =>
            assertableTask.AssertIsNotErroneous();
    }
}
