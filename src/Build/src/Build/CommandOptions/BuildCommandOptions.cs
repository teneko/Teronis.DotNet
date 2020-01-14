using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(COMMAND)]
    public class BuildCommandOptions : ICommandOptions
    {
        public const string COMMAND = "build";

        public string Command => COMMAND;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
