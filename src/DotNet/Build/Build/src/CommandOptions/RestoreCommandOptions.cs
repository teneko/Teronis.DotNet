using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Teronis.DotNet.Build.CommandOptions
{
    [Verb(RestoreCommand)]
    public class RestoreCommandOptions : CommandOptionsBase
    {
        public const string RestoreCommand = "restore";

        public override string Command => RestoreCommand;
    }
}
