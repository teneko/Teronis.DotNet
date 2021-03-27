// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Teronis.NUnit.TaskTests
{
    public class TaskTestCase : LazyTask, ITestCaseData
    {
        public object? ExpectedResult =>
            testCaseData.ExpectedResult;

        public bool HasExpectedResult =>
            testCaseData.HasExpectedResult;

        public string? TestName =>
            testCaseData.TestName;

        public RunState RunState =>
            testCaseData.RunState;

        public object?[] Arguments =>
            new object?[] { Value };

        public IPropertyBag Properties =>
            testCaseData.Properties;

        private TestCaseData testCaseData;

        public TaskTestCase(Func<CancellationToken, Task> taskProvider, TestCaseData testCaseData)
            : base(taskProvider) =>
            this.testCaseData = testCaseData ?? throw new ArgumentNullException(nameof(testCaseData));

        /// <summary>
        /// Let's you configure the internal residing <see cref="TestCaseData"/>.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public TaskTestCase ConfigureTestCaseData(Action<TestCaseDataConfigurator> configure)
        {
            if (configure is null) {
                throw new ArgumentNullException(nameof(configure));
            }

            configure(new TestCaseDataConfigurator(testCaseData));
            return this;
        }

        internal TaskTestCaseParameters ToTestCaseParameters() =>
            new TaskTestCaseParameters(this);
    }
}
