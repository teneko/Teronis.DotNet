using CommandLine;

namespace Teronis.DotNet.Build
{
    interface ICommandOptions
    {
        public const string ConfigurationLongName = "configuration";
        public const string VerbosityLongName = "verbosity";
        public const string DryRunLongName = "dry-run";
        public const string SkipDependenciesLongName = "skip-dependencies";

        string Command { get; }

        [Option(ConfigurationLongName, Default = "Release")]
        string? Configuration { get; set; }

        [Option(VerbosityLongName, Default = "normal")]
        string? Verbosity { get; set; }

        /// <summary>
        /// The affected command will be printed out.
        /// </summary>
        [Option(DryRunLongName, Default = false)]
        bool DryRun { get; set; }

        [Option(SkipDependenciesLongName, Default = false)]
        bool SkipDependencies { get; set; }
    }
}
