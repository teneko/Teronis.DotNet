// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnLocalObjectAttribute : Attribute
    {
        public string? NameOrPath { get; }

        public ReturnLocalObjectAttribute()
        { }

        public ReturnLocalObjectAttribute(string localObjectNameOrPath) =>
            NameOrPath = localObjectNameOrPath;
    }
}
