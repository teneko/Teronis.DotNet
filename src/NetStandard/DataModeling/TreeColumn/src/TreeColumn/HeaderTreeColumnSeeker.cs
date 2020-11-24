using System;
using Teronis.DataModeling.TreeColumn.Core;

namespace Teronis.DataModeling.TreeColumn
{
    public class HeaderTreeColumnSeeker : TreeColumnSeekerBase<HeaderTreeColumnKey, HeaderTreeColumnValue>
    {
        public HeaderTreeColumnSeeker(Type treeColumnsHolderType) : base(treeColumnsHolderType) { }

        protected override HeaderTreeColumnValue instantiateTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            => new HeaderTreeColumnValue(key, path, index);
    }
}
