// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.ObjectModel.TreeColumn.Core;

namespace Teronis.ObjectModel.TreeColumn
{
    public class HeaderTreeColumnValue : TreeColumnValue<HeaderTreeColumnKey>
    {
        public HeaderTreeColumnValue(HeaderTreeColumnKey key, string path, int index)
            : base(key, path, index) { }
    }
}
