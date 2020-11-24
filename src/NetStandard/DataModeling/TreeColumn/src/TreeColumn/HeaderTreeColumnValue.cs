using Teronis.DataModeling.TreeColumn.Core;

namespace Teronis.DataModeling.TreeColumn
{
    public class HeaderTreeColumnValue : TreeColumnValue<HeaderTreeColumnKey>
    {
        public HeaderTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            : base(key, path, index) { }
    }
}
