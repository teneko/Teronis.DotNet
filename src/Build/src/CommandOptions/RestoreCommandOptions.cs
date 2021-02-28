using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(RestoreCommand, HelpText = "Restores all projects")]
    public class RestoreCommandOptions : CommandOptionsBase
    {
        public const string RestoreCommand = "restore";

        public override string Command => RestoreCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsRestoreCommand(this ICommandOptions options) => 
            options.Command == RestoreCommandOptions.RestoreCommand;
    }
}
