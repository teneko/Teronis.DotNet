// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using NUnit.Common;
using NUnit.Framework.Interfaces;
using NUnitLite;

namespace Teronis.NUnit.Api
{
    public static class ITestResultExtensions
    {
        /// <summary>
        /// Generates the xml report of <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The test result.</param>
        /// <returns>
        /// The result of <see cref="IXmlNodeBuilder.ToXml(bool)"/>
        /// of <see cref="ITestResult"/>. Null if 
        /// <paramref name="result"/> is null. Empty
        /// if no skip or failure has been occured.
        /// </returns>
        [return: NotNullIfNotNull("result")]
        public static string? GenerateXmlReport(this ITestResult? result)
        {
            if (result is null) {
                return null;
            }

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter);
            result.ToXml(recursive: true).WriteTo(xmlWriter);
            return stringWriter.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Generates an human readable summary report.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>
        /// The result report generated from
        /// <see cref="TextUI.DisplaySummaryReport(ResultSummary)"/>.
        /// </returns>
        [return: NotNullIfNotNull("result")]
        public static string? GenerateSummaryReport(this ITestResult? result)
        {
            if (result is null) {
                return null;
            }

            using var stringWriter = new StringWriter();
            var textUI = new TextUI(new ExtendedTextWrapper(stringWriter), reader: null, new NUnitLiteOptions());
            textUI.DisplaySummaryReport(new ResultSummary(result));
            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
