// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace Teronis.ModuleInitializer.AssemblyLoader
{
    [Verb("inject-assembly-loader")]
    public class InjectAssemblyLoaderCommandOptions
    {
        public const string SourceAssemblyPathOptionLongName = "source-assembly-path";

        [Value(0, MetaName = "injection target assembly path", Required = true)]
        public string InjectionTargetAssemblyPath { get; set; } = null!;
        [Option(SourceAssemblyPathOptionLongName, HelpText = "Loads from specified path the the module initializers you want to have called in injection target assembly", Required = true)]
        public string SourceAssemblyPathToBeLoaded { get; set; } = null!;
    }
}
