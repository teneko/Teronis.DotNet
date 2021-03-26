// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Diagnostics;

namespace Teronis.GitVersion.CommandLine
{
    public class GitVersionProcess : SimpleAsyncProcess
    {
        public GitVersionProcess(
            GitVersionProcessStartInfo processStartInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
            : base(
                  processStartInfo,
                  echoCommand,
                  commandEchoPrefix,
                  outputReceived,
                  errorReceived) 
        { }
    }
}
