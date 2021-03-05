using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Teronis.Diagnostics;

namespace Teronis.GitVersion.CommandLine
{
    public class GitVersionCommandLineLibrary
    {
        private const string GitVersionExecutableName = "GitVersion.exe";

        private static string evaluateGitVersionExecutablePath()
        {
            var executingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var gitVersionExecutablePath = Path.Combine(executingAssemblyDirectory, GitVersionExecutableName);
            return gitVersionExecutablePath;
        }

        public static string ExecuteGitVersion(string? args = null, string? workingDirectory = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string?>? errorReceived = null) =>
            SimpleProcess.Read(
                evaluateGitVersionExecutablePath(),
                args: args, 
                workingDirectory: workingDirectory, 
                echoCommand: echoCommand, 
                commandEchoPrefix: commandEchoPrefix, 
                errorReceived: errorReceived);

        public static ValueTask<string> ExecuteGitVersionAsync(string? args = null, string? workingDirectory = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string?>? errorReceived = null) =>
            SimpleProcess.ReadAsync(evaluateGitVersionExecutablePath(), 
                args: args, 
                workingDirectory: workingDirectory, 
                echoCommand: echoCommand, 
                commandEchoPrefix: commandEchoPrefix, 
                errorReceived: errorReceived);
    }
}
