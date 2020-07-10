using Teronis.Data.TreeColumn.Core;

namespace Teronis.Data.TreeColumn
{
    public class HeaderTreeColumnValue : TreeColumnValue<HeaderTreeColumnKey>
    {
        public HeaderTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            : base(key, path, index) { }
    }
}
