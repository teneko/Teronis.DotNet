

namespace Teronis.DotNet.Build.CommandOptions
{
    public abstract class CommandOptionsBase : ICommandOptions
    {
        public abstract string Command { get; }
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
        /// <summary>
        /// The affected command will be printed out.
        /// </summary>
        public bool DryRun { get; set; }
    }
}
