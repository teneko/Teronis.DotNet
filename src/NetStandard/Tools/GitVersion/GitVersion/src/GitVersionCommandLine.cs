using System.Threading.Tasks;
using Teronis.Diagnostics;

namespace Teronis.Utils.GitVersion
{
    public class GitVersionCommandLine
    {
        private const string GitVersionExecutableName = "GitVersion.exe";

        public static string ExecuteGitVersion(string? args = null) =>
            SimpleProcess.Read(GitVersionExecutableName, args);

        public static ValueTask<string> ExecuteGitVersionAsync(string? args = null) =>
            SimpleProcess.ReadAsync(GitVersionExecutableName, args);
    }
}
