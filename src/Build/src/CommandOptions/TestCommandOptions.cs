using CommandLine;

namespace Teronis.Build.CommandOptions
{
    [Verb(TestCommand, HelpText = "Restores and builds synthetic projects and restores, builds and tests non-synthetic projects")]
    public class TestCommandOptions : CommandOptionsBase
    {
        public const string TestCommand = "test";

        public override string Command => TestCommand;
    }

    partial class CommandOptionsExtensions
    {
        public static bool IsTestCommand(this ICommandOptions options) => 
            options.Command == TestCommandOptions.TestCommand;
    }
}
