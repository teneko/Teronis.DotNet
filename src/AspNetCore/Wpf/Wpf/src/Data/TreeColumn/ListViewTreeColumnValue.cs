using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Teronis.Data.TreeColumn.Core;

namespace Teronis.Data.TreeColumn
{
    public class ListViewTreeColumnValue : TreeColumnValue<ListViewTreeColumnKey>
    {
        public DataTemplate DataTemplate { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; set; }
        public Binding Binding { get; private set; }

        public override string Path => Binding.Path.Path;

        public ListViewTreeColumnValue(ListViewTreeColumnKey key, Binding binding, int index)
        {
            Key = key;
            Binding = binding;
            Index = index;
        }
    }
}
