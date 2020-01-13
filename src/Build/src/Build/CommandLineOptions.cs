using CommandLine;

namespace Teronis.DotNet.Build
{
    class CommandLineOptions
    {
        public const string ConfigurationName = "--configuration";
        public const string VerbosityName = "--verbosity";

        [Option(ConfigurationName, Default = "Release")]
        public string Configuration { get; set; }

        [Option(VerbosityName, Default = "normal")]
        public string Verbosity { get; set; }
    }
}
