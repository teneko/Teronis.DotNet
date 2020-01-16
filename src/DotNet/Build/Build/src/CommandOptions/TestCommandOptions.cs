using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(TestCommand)]
    public class TestCommandOptions : ICommandOptions
    {
        public const string TestCommand = "test";

        public string Command => TestCommand;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
