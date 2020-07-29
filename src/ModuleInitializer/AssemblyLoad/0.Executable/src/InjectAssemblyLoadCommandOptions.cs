﻿using CommandLine;

namespace Teronis.ModuleInitializer.AssemblyLoad
{
    [Verb("inject-assembly-load")]
    public class InjectAssemblyLoadCommandOptions
    {
        public const string SourceAssemblyPathOptionLongName = "source-assembly-path";

        [Value(0, MetaName = "injection target assembly path", Required = true)]
        public string InjectionTargetAssemblyPath { get; set; } = null!;
        [Option(SourceAssemblyPathOptionLongName, HelpText = "Loads from specified path the the module initializers you want to have called in injection target assembly", Required = true)]
        public string SourceAssemblyPathToBeLoaded { get; set; } = null!;
    }
}
