// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.ObjectModel.TreeColumn.Core;

namespace Teronis.ObjectModel.TreeColumn
{
    public class HeaderTreeColumnKey : TreeColumnKey
    {
        public string? Header { get; private set; }

        public HeaderTreeColumnKey(Type declarationType, string variableName, string? header)
            : base(declarationType, variableName)
        {
            Header = header;
        }

        public HeaderTreeColumnKey(Type declarationType, string variableName)
            : this(declarationType, variableName, null) { }
    }
}
