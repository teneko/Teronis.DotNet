// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Reflection;
using Teronis.Diagnostics;

namespace Teronis.GitVersion.CommandLine
{
    public class GitVersionProcessStartInfo : SimpleProcessStartInfo
    {
        private const string GitVersionExecutableName = "GitVersion.exe";

        private static string EvaluateGitVersionExecutablePath()
        {
            var executingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var gitVersionExecutablePath = Path.Combine(executingAssemblyDirectory, GitVersionExecutableName);
            return gitVersionExecutablePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">If null the absolute executable path will be evaluated.</param>
        /// <param name="args"></param>
        /// <param name="workingDirectory"></param>
        public GitVersionProcessStartInfo(
            string? name = null,
            string? args = null,
            string? workingDirectory = null) :
            base(
                name ?? EvaluateGitVersionExecutablePath(),
                args,
                workingDirectory)
        { }
    }
}
