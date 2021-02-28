using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(BuildSyntheticCommand, HelpText = "Restores and builds synthetic projects")]
    public class BuildSyntheticCommandOptions : CommandOptionsBase
    {
        public const string BuildSyntheticCommand = "build-synthetic";

        public override string Command => BuildSyntheticCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsBuildSyntheticCommand(this ICommandOptions options) => 
            options.Command == BuildSyntheticCommandOptions.BuildSyntheticCommand;
    }
}
