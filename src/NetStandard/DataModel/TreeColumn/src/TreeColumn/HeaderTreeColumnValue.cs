using Teronis.DataModel.TreeColumn.Core;

namespace Teronis.DataModel.TreeColumn
{
    public class HeaderTreeColumnValue : TreeColumnValue<HeaderTreeColumnKey>
    {
        public HeaderTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            : base(key, path, index) { }
    }
}
