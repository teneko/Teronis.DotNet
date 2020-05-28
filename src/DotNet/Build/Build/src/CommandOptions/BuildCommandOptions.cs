using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(BuildCommand)]
    public class BuildCommandOptions : CommandOptionsBase
    {
        public const string BuildCommand = "build";

        public override string Command => BuildCommand;
    }
}
