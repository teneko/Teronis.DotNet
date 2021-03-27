// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Teronis.NUnit.Api;

namespace Teronis.NUnit.TaskTests
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
    public class TaskTestCaseSourceTestFixture
    {
        /// <summary>
        /// <para>
        /// If this (derived) class is target of
        /// for example <see cref="NUnitSingleThreadAssemblyRunner"/>
        /// you should initialize <see cref="TaskTestCases"/>.
        /// </para>
        /// Items of enumerable can consist of
        /// <br/><see cref="TaskTestCaseParameters"/>
        /// <br/><see cref="TestCaseParameters"/> and
        /// <br/><see cref="Array"/>/<see cref="object"/>[]
        /// </summary>
        public static IEnumerable? TaskTestCases = null!;

        [TestCaseSource(typeof(TaskTestCaseSourceTestFixture), nameof(TaskTestCases))]
        public virtual void AssertTask(Task assertableTask) =>
            assertableTask.AssertIsNotErroneous();
    }
}
