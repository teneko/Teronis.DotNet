using CommandLine;

namespace Teronis.Build
{
    interface ICommandOptions
    {
        public const string ConfigurationLongName = "configuration";
        public const string VerbosityLongName = "verbosity";
        public const string DryRunLongName = "dry-run";
        public const string SkipDependenciesLongName = "skip-dependencies";
        //public const string MSBuildPropertiesLongName = "msbuild-properties";

        string Command { get; }

        [Option('c', ConfigurationLongName, Default = "Release")]
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

        //[Option('p', MSBuildPropertiesLongName)]
        //string? MSBuildProperties { get; set; }
    }
}
