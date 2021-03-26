// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Teronis.Text;

namespace Teronis.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Joins the message of <paramref name="exception"/> and the message of <see cref="Exception.InnerException"/> recursively.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="separator">If null it is <see cref="Environment.NewLine"/></param>
        /// <returns></returns>
        public static string JoinInnerMessages(this Exception? exception, string? separator = null)
        {
            separator ??= Environment.NewLine;
            var stringBuilder = new StringBuilder();
            var stringSeparationHelper = new StringSeparator(separator);

            var currentException = exception;

            while (!(currentException is null)) {
                if (!string.IsNullOrWhiteSpace(currentException.Message)) {
                    stringBuilder.Append(currentException.Message.Trim());
                    stringSeparationHelper.SetSeparator(stringBuilder);
                }

                if (currentException.InnerException is null) {
                    break;
                }

                currentException = currentException.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}
