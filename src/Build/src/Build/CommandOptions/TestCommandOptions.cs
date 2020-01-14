using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(COMMAND)]
    public class TestCommandOptions : ICommandOptions
    {
        public const string COMMAND = "test";
        public string Command => COMMAND;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
