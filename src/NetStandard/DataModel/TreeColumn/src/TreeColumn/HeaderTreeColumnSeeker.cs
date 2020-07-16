using System;
using Teronis.DataModel.TreeColumn.Core;

namespace Teronis.DataModel.TreeColumn
{
    public class HeaderTreeColumnSeeker : TreeColumnSeekerBase<HeaderTreeColumnKey, HeaderTreeColumnValue>
    {
        public HeaderTreeColumnSeeker(Type treeColumnsHolderType) : base(treeColumnsHolderType) { }

        protected override HeaderTreeColumnValue instantiateTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            => new HeaderTreeColumnValue(key, path, index);
    }
}
