using CommandLine;

namespace Teronis.NetCoreApp.AssemblyLoadInjection.Executable
{
    [Verb("inject-assembly-initializer")]
    public class InjectAssemblyInitializerCommandOptions
    {
        public const string AssemblyNameOptionLongName = "assembly-name";
        public const string AssemblyNameFromPathOptionLongName = "assembly-name-from-path";

        [Value(0, MetaName = "injection target assembly path", Required = true)]
        public string InjectionTargetAssemblyPath { get; set; } = null!;
        [Option(AssemblyNameOptionLongName, HelpText = "The (full) assemly name you want to have initialized in injection target assembly")]
        public string AssemlyNameToBeLoaded { get; set; } = null!;
        [Option(AssemblyNameFromPathOptionLongName, HelpText = "Loads from specified path the the full assembly name you want to have initialized in injection target assembly")]
        public string AssemlyNameFromPathToBeLoaded { get; set; } = null!;
    }
}
