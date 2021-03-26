// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Teronis.ObjectModel.TreeColumn.Core;

namespace Teronis.Windows.PresentationFoundation.Data.TreeColumn
{
    public class ListViewTreeColumnValue : TreeColumnValue<ListViewTreeColumnKey>
    {
        public DataTemplate DataTemplate { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; set; }
        public Binding Binding { get; private set; }

        public override string Path => Binding.Path.Path;

        public ListViewTreeColumnValue(ListViewTreeColumnKey key, Binding binding, int index)
            : base(key, null!, index) =>
            Binding = binding;
    }
}
