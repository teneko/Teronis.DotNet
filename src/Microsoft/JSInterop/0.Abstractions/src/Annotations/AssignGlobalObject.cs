// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AssignGlobalObject : Attribute
    {
        public string? GlobalObjectName { get; }

        public AssignGlobalObject()
        { }

        public AssignGlobalObject(string globalObjectName) =>
            GlobalObjectName = globalObjectName ?? throw new ArgumentNullException(nameof(globalObjectName));
    }
}
