using CommandLine;

namespace Teronis.DotNet.Build
{
    interface ICommandOptions
    {
        public const string ConfigurationLongName = "configuration";
        public const string VerbosityLongName = "verbosity";
        public const string DryRunLongName = "dryrun";

        string Command { get; }

        [Option(ConfigurationLongName, Default = "Release")]
        string Configuration { get; set; }

        [Option(VerbosityLongName, Default = "normal")]
        string Verbosity { get; set; }

        [Option(DryRunLongName, Default = false)]
        bool DryRun { get; set; }
    }
}
