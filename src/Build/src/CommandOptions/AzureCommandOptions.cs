using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(AzureCommand, HelpText = "Restores, builds, tests and packs projects")]
    public class AzureCommandOptions : CommandOptionsBase
    {
        public const string AzureCommand = "azure";

        public override string Command => AzureCommand;
    }
}
