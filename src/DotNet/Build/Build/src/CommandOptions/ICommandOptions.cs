using CommandLine;

namespace Teronis.DotNet.Build
{
    interface ICommandOptions
    {
        public const string ConfigurationLongName = "configuration";
        public const string VerbosityLongName = "verbosity";

        string Command { get; }

        [Option(ConfigurationLongName, Default = "Release", ResourceType = typeof(string))]
        string Configuration { get; set; }

        [Option(VerbosityLongName, Default = "normal", ResourceType = typeof(string))]
        string Verbosity { get; set; }
    }
}
