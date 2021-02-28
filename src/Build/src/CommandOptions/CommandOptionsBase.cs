namespace Teronis.Build.CommandOptions
{
    public abstract partial class CommandOptionsBase : ICommandOptions
    {
        public abstract string Command { get; }
        public string? Configuration { get; set; }
        public string? Verbosity { get; set; }
        public bool DryRun { get; set; }
        public bool SkipDependencies { get; set; }
    }
}
