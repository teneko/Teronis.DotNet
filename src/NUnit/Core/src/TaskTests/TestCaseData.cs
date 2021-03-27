using NUnit.Framework;

namespace Teronis.NUnit.TaskTests
{
    public struct TestCaseDataConfigurator
    {
        private readonly TestCaseData testCaseData;

        internal TestCaseDataConfigurator(TestCaseData testCaseData) =>
            this.testCaseData = testCaseData;

        /// <summary>
        /// Marks the test case as explicit.
        /// </summary>
        /// <returns></returns>
        public TestCaseData Explicit() =>
            testCaseData.Explicit();

        /// <summary>
        /// Marks the test case as explicit, specifying the reason.
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public TestCaseData Explicit(string reason) =>
            testCaseData.Explicit(reason);

        /// <summary>
        /// Ignores this TestCase, specifying the reason..
        /// </summary>
        /// <param name="reason">The ignore reason.</param>
        /// <returns></returns>
        public IgnoredTestCaseData Ignore(string reason) =>
            testCaseData.Ignore(reason);

        /// <summary>
        /// Sets the expected result for the test
        /// </summary>
        /// <param name="result">The expected result.</param>
        /// <returns>The modified <see cref="TestCaseDataConfigurator"/>.</returns>
        public TestCaseData Returns(object? result) =>
            testCaseData.Returns(result);

        /// <summary>
        /// Sets the list of display names to use as the parameters in the test name.
        /// </summary>
        /// <param name="displayNames"></param>
        /// <returns></returns>
        public TestCaseData SetArgDisplayNames(params string[]? displayNames) =>
            testCaseData.SetArgDisplayNames(displayNames);

        /// <summary>
        /// Applies a category to the test.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public TestCaseData SetCategory(string category) =>
            testCaseData.SetCategory(category);

        /// <summary>
        /// Sets the description for the test case being constructed.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The modified TestCaseData instance.</returns>
        public TestCaseData SetDescription(string description) =>
            testCaseData.SetDescription(description);

        /// <summary>
        /// Sets the name of the test case.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData SetName(string? name) =>
            testCaseData.SetName(name);

        /// <summary>
        /// Applies a named property to the test.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, string propValue) =>
            testCaseData.SetProperty(propName, propValue);

        /// <summary>
        /// Applies a named property to the test.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, int propValue) =>
            testCaseData.SetProperty(propName, propValue);

        /// <summary>
        /// Applies a named property to the test.
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, double propValue) =>
            testCaseData.SetProperty(propName, propValue);
    }
}
