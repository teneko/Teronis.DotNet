using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(COMMAND)]
    public class PackCommandOptions : ICommandOptions
    {
        public const string COMMAND = "pack";
        public string Command => COMMAND;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
