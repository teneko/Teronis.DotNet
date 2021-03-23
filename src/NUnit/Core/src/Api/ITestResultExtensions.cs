﻿using System.Diagnostics.CodeAnalysis;
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
        /// Generates an human readable errors, failures and warnings report. It may be empty when
        /// no such error, failures or warnings are part of the result.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>
        /// The result report generated from 
        /// <see cref="TextUI.DisplayErrorsFailuresAndWarningsReport(ITestResult)"/>.
        /// </returns>
        [return: NotNullIfNotNull("result")]
        public static string? GenerateErrorsFailuresAndWarningsReport(this ITestResult? result) {
            if (result is null) {
                return null;
            }

            using var stringWriter = new StringWriter();
            var textUI = new TextUI(new ExtendedTextWrapper(stringWriter), reader: null, new NUnitLiteOptions());
            textUI.DisplayErrorsFailuresAndWarningsReport(result);
            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
