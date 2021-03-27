// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using NUnitTest = NUnit.Framework.Internal.Test;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Use this attribute when you only want to test
    /// a single static instance member of <see cref="ITaskTestCaseBlock"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TaskTestCaseBlockStaticMemberSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        public Type TaskTestsClassType { get; }
        public string TaskTestsMemberName { get; }

        private List<TestCaseParameters> skipReasonedParametersList;
        private List<TestCaseParameters> testCaseParametersList;

        /// <summary>
        /// Creates an attribute that is recognized by NUnit.
        /// </summary>
        /// <param name="taskTestsClassType">The <see cref="TaskTestCaseBlock"/> class type that has a static member of type <see cref="ITaskTestCaseBlock"/>.</param>
        /// <param name="staticMemberName">The name of the static member that implements <see cref="ITaskTestCaseBlock"/>.</param>
        public TaskTestCaseBlockStaticMemberSourceAttribute(Type taskTestsClassType, string staticMemberName)
        {
            skipReasonedParametersList = new List<TestCaseParameters>();
            testCaseParametersList = new List<TestCaseParameters>();

            TaskTestsClassType = taskTestsClassType;
            TaskTestsMemberName = staticMemberName;

            if (!TryGetStaticMemberType(out var testAssertions)) {
                return;
            }

            AddTestAssertionsToTestCaseParametersList(testAssertions);
        }

        private void AddSkipReason(string errorMessage)
        {
            var erroneousValidationParameters = new TestCaseParameters();
            erroneousValidationParameters.RunState = RunState.NotRunnable;
            erroneousValidationParameters.Properties.Set(PropertyNames.SkipReason, errorMessage);
            skipReasonedParametersList.Add(erroneousValidationParameters);
        }

        private bool IsTypeAssignableToTestAssertionsType(Type type)
        {
            if (!typeof(ITaskTestCaseBlock).IsAssignableFrom(type)) {
                AddSkipReason($"Source type has to be assignable to {typeof(ITaskTestCaseBlock)}.");
                return false;
            }

            return true;
        }

        private bool TryGetStaticMemberType([MaybeNullWhen(false)] out ITaskTestCaseBlock taskTests)
        {
            var taskTestsMemberInfos = TaskTestsClassType.GetMember(TaskTestsMemberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (taskTestsMemberInfos.Length != 1) {
                AddSkipReason($"More than one member by name \"{TaskTestsMemberName}\" exist.");
                goto @return;
            }

            var taskTestsMemberInfo = taskTestsMemberInfos[0];
            object? taskTestsInstanceObject;

            if (taskTestsMemberInfo.MemberType == MemberTypes.Field) {
                taskTestsInstanceObject = ((FieldInfo)taskTestsMemberInfo).GetValue(null);
            } else if (taskTestsMemberInfo.MemberType == MemberTypes.Property) {
                taskTestsInstanceObject = ((PropertyInfo)taskTestsMemberInfo).GetValue(null);
            } else {
                AddSkipReason($"Member by name \"{TaskTestsMemberName}\" is not field or property.");
                goto @return;
            }

            if (taskTestsInstanceObject is null) {
                AddSkipReason($"Member by name \"{TaskTestsMemberName}\" is null.");
                goto @return;
            }

            if (!IsTypeAssignableToTestAssertionsType(taskTestsInstanceObject.GetType())) {
                AddSkipReason($"Member by name \"{TaskTestsMemberName}\" is not of type \"{typeof(ITaskTestCaseBlock)}\".");
                goto @return;
            }

            taskTests = (ITaskTestCaseBlock)taskTestsInstanceObject;
            return true;

            @return:
            taskTests = null;
            return false;
        }

        private void AddTestAssertionsToTestCaseParametersList(ITaskTestCaseBlock taskTestsInstance)
        {
            foreach (var testCase in taskTestsInstance.GetTestCases()) {
                var testCaseParameters = new TestCaseParameters(testCase);
                testCaseParametersList.Add(testCaseParameters);
            }
        }

        private IEnumerable<TestMethod> BuildTestMethods(
            IMethodInfo testMethodInfo,
            NUnitTest? suite,
            IEnumerable<TestCaseParameters> testCaseParametersEnumerable)
        {
            var testCaseParametersEnumerator = testCaseParametersEnumerable.GetEnumerator();
            var testCaseBuilder = new NUnitTestCaseBuilder();

            while (testCaseParametersEnumerator.MoveNext()) {
                var testCaseParameters = testCaseParametersEnumerator.Current;
                yield return testCaseBuilder.BuildTestMethod(testMethodInfo, suite, testCaseParameters);
            }
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo testMethodInfo, NUnitTest? suite)
        {
            if (skipReasonedParametersList.Count != 0) {
                return BuildTestMethods(testMethodInfo, suite, skipReasonedParametersList);
            } else {
                return BuildTestMethods(testMethodInfo, suite, testCaseParametersList);
            }
        }
    }
}
