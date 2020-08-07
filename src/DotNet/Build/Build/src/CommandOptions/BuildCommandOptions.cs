using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(BuildCommand, HelpText = "Builds projects")]
    public class BuildCommandOptions : CommandOptionsBase
    {
        public const string BuildCommand = "build";

        public override string Command => BuildCommand;
    }
}
