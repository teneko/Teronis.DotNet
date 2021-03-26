// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Teronis.NetStandard.Collections")]
[assembly: IgnoresAccessChecksTo("Teronis.Nullable")]

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
internal sealed class IgnoresAccessChecksToAttribute : Attribute
{
    public IgnoresAccessChecksToAttribute(string assemblyName)
    {
        AssemblyName = assemblyName;
    }

    public string AssemblyName { get; }
}
