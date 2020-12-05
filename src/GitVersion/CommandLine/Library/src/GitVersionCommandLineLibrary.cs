using System;
using System.Threading.Tasks;
using Teronis.Diagnostics;

namespace Teronis.GitVersion.CommandLine
{
    public class GitVersionCommandLineLibrary
    {
        private const string GitVersionExecutableName = "GitVersion.exe";

        public static string ExecuteGitVersion(string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? errorReceived = null) =>
            SimpleProcess.Read(GitVersionExecutableName, args);

        public static ValueTask<string> ExecuteGitVersionAsync(string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? errorReceived = null) =>
            SimpleProcess.ReadAsync(GitVersionExecutableName, args);
    }
}
