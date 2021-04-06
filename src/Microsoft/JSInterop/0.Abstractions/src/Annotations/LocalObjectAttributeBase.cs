// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LocalObjectAttributeBase : Attribute
    {
        /// <summary>
        /// When specified the name or path gets precedence over 
        /// the default member name or JavaScript identifier name.
        /// </summary>
        public string? NameOrPath { get; set; }
    }
}
