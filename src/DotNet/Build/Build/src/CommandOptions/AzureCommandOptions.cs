using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(AzureCommand)]
    public class AzureCommandOptions : CommandOptionsBase
    {
        public const string AzureCommand = "azure";

        public override string Command => AzureCommand;
    }
}
