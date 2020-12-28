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
