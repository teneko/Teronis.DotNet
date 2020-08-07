using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(PackCommand, HelpText = "Packs projects")]
    public class PackCommandOptions : CommandOptionsBase
    {
        public const string PackCommand = "pack";

        public override string Command => PackCommand;
    }
}
