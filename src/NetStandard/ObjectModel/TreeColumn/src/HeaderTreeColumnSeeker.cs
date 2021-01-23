using System;
using Teronis.ObjectModel.TreeColumn.Core;

namespace Teronis.ObjectModel.TreeColumn
{
    public class HeaderTreeColumnSeeker : TreeColumnSeekerBase<HeaderTreeColumnKey, HeaderTreeColumnValue>
    {
        public HeaderTreeColumnSeeker(Type treeColumnsHolderType) : base(treeColumnsHolderType) { }

        protected override HeaderTreeColumnValue instantiateTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            => new HeaderTreeColumnValue(key, path, index);
    }
}
