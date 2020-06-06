using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    public abstract class CommandOptionsBase : ICommandOptions
    {
        public abstract string Command { get; }
        public string? Configuration { get; set; }
        public string? Verbosity { get; set; }
        public bool DryRun { get; set; }
        public bool NoDependencies { get; set; }
    }
}
