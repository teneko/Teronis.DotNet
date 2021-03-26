// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text;

namespace Teronis.Diagnostics
{
    public static class ProcessStartInfoUtils
    {
        public static string? GetExecutionInfoText(ProcessStartInfo startInfo, string? commandEchoPrefix = null)
        {
            if (startInfo == null) {
                return null;
            }

            var echoBuilder = new StringBuilder();

            void appendCommandEchoPrefix()
            {
                if (commandEchoPrefix != null) {
                    echoBuilder.Append($"{commandEchoPrefix}: ");
                }
            }

            if (!string.IsNullOrEmpty(startInfo.WorkingDirectory)) {
                appendCommandEchoPrefix();
                echoBuilder.Append($"Working directory: {startInfo.WorkingDirectory}{Environment.NewLine}");
            }

            appendCommandEchoPrefix();
            echoBuilder.Append($"{startInfo.FileName} {startInfo.Arguments}");
            var commandEcho = echoBuilder.ToString();
            return commandEcho;
        }
    }
}
