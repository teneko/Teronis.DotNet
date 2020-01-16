using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(BuildCommand)]
    public class BuildCommandOptions : ICommandOptions
    {
        public const string BuildCommand = "build";

        public string Command => BuildCommand;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
