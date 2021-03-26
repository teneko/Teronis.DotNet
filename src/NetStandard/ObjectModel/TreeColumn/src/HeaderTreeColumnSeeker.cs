// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
