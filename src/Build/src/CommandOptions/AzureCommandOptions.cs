using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(AzureCommand, HelpText = "Restores and builds synthetic projects and restores, builds, tests and packs non-synthetic projects")]
    public class AzureCommandOptions : CommandOptionsBase
    {
        public const string AzureCommand = "azure";

        public override string Command => AzureCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsAzureCommand(this ICommandOptions options) => 
            options.Command == AzureCommandOptions.AzureCommand;
    }
}
