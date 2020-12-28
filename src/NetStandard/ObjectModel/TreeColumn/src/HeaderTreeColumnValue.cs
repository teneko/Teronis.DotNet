using Teronis.ObjectModel.TreeColumn.Core;

namespace Teronis.ObjectModel.TreeColumn
{
    public class HeaderTreeColumnValue : TreeColumnValue<HeaderTreeColumnKey>
    {
        public HeaderTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            : base(key, path, index) { }
    }
}
