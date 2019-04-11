using System;
using Teronis.Data.TreeColumn.Core;

namespace Teronis.Data.TreeColumn
{
    public class HeaderTreeColumnSeeker : TreeColumnSeekerBase<HeaderTreeColumnKey, HeaderTreeColumnValue>
    {
        public HeaderTreeColumnSeeker(Type MightOwnTreeColumnsType) : base(MightOwnTreeColumnsType) { }

        protected override HeaderTreeColumnValue instantiateTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            => new HeaderTreeColumnValue(key, path, index);
    }
}
