// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Linq;
using NUnit.Common;
using NUnit.Framework.Interfaces;
using NUnitLite;

namespace Teronis.NUnit.Api
{
    public static class ITestResultExtensions
    {
        /// <summary>
        /// Generates the XML report of <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The test result.</param>
        /// <param name="ident">Idents XML output.</param>
        /// <returns>
        /// The result of <see cref="IXmlNodeBuilder.AddToXml(TNode, bool)"/>
        /// of <see cref="ITestResult"/>. The root node is &lt;tests/&gt;.
        /// </returns>
        public static string GenerateXmlReport(this ITestResult? result, bool ident = false)
        {
            var node = new TNode("tests");

            if (!(result is null)) {
                result.AddToXml(node, recursive: true);
            }

            return ident
                ? XDocument.Parse(node.OuterXml).ToString()
                : node.OuterXml;
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
