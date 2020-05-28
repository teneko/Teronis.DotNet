using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(TestCommand)]
    public class TestCommandOptions : CommandOptionsBase
    {
        public const string TestCommand = "test";

        public override string Command => TestCommand;
    }
}
