using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(PackCommand, HelpText = "Restores and builds synthesized projects and restores, builds and packs non-synthetic projects")]
    public class PackCommandOptions : CommandOptionsBase
    {
        public const string PackCommand = "pack";

        public override string Command => PackCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsPackCommand(this ICommandOptions options) => 
            options.Command == PackCommandOptions.PackCommand;
    }
}
