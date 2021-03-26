// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace Teronis.ModuleInitializer.AssemblyLoader
{
    [Verb("read-assembly-name")]
    public class ReadFullAssemblyNameCommand
    {
        [Value(2, MetaName = "assembly path", Required = true, HelpText = "Assmebly path from which the full name will be taken")]
        public string AssemblyPath { get; set; } = null!;
    }
}
