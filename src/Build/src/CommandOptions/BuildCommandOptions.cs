using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(BuildCommand, HelpText = "Restores and builds synthetic and non-synthetic projects")]
    public class BuildCommandOptions : CommandOptionsBase
    {
        public const string BuildCommand = "build";

        public override string Command => BuildCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsBuildCommand(this ICommandOptions options) => 
            options.Command == BuildCommandOptions.BuildCommand;
    }
}
