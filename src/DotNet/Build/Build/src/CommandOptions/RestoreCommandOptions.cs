using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(RestoreCommand, HelpText = "Restores projects")]
    public class RestoreCommandOptions : CommandOptionsBase
    {
        public const string RestoreCommand = "restore";

        public override string Command => RestoreCommand;
    }
}
