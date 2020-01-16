using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(PackCommand)]
    public class PackCommandOptions : ICommandOptions
    {
        public const string PackCommand = "pack";

        public string Command => PackCommand;
        public string Configuration { get; set; }
        public string Verbosity { get; set; }
    }
}
