using System;
using Teronis.DataModeling.TreeColumn.Core;

namespace Teronis.DataModeling.TreeColumn
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
