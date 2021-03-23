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
    /// a single instance of <see cref="ITaskTests"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TaskTestsSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        public Type TaskTestsClassType { get; }
        public string TaskTestsInstanceName { get; }

        private List<TestCaseParameters> skipReasonedParametersList;
        private List<TestCaseParameters> testCaseParametersList;

        public TaskTestsSourceAttribute(Type taskTestsClassType, string taskTestsInstanceName)
        {
            skipReasonedParametersList = new List<TestCaseParameters>();
            testCaseParametersList = new List<TestCaseParameters>();

            TaskTestsClassType = taskTestsClassType;
            TaskTestsInstanceName = taskTestsInstanceName;

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
            if (!typeof(ITaskTests).IsAssignableFrom(type)) {
                AddSkipReason($"Source type has to be assignable to {typeof(ITaskTests)}.");
                return false;
            }

            return true;
        }

        private bool TryGetStaticMemberType([MaybeNullWhen(false)] out ITaskTests testAssertions)
        {
            var members = TaskTestsClassType.GetMember(TaskTestsInstanceName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (members.Length != 1) {
                AddSkipReason($"More than one member by name \"{TaskTestsInstanceName}\" exist.");
                goto @return;
            }

            var testAssertionsMember = members[0];
            object? testAssertionsObject;

            if (testAssertionsMember.MemberType == MemberTypes.Field) {
                testAssertionsObject = ((FieldInfo)testAssertionsMember).GetValue(null);
            } else if (testAssertionsMember.MemberType == MemberTypes.Property) {
                testAssertionsObject = ((PropertyInfo)testAssertionsMember).GetValue(null);
            } else {
                AddSkipReason($"Member by name \"{TaskTestsInstanceName}\" is not field or property.");
                goto @return;
            }

            if (testAssertionsObject is null) {
                AddSkipReason($"Member by name \"{TaskTestsInstanceName}\" is null.");
                goto @return;
            }

            if (!IsTypeAssignableToTestAssertionsType(testAssertionsObject.GetType())) {
                AddSkipReason($"Member by name \"{TaskTestsInstanceName}\" is not of type \"{typeof(ITaskTests)}\".");
                goto @return;
            }

            testAssertions = (ITaskTests)testAssertionsObject;
            return true;

            @return:
            testAssertions = null;
            return false;
        }

        private void AddTestAssertionsToTestCaseParametersList(ITaskTests taskTestsInstance)
        {
            foreach (var testAssertion in taskTestsInstance.GetAssertableTasks()) {
                var testCaseParameters = new TestCaseParameters(new object[] { testAssertion });
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
