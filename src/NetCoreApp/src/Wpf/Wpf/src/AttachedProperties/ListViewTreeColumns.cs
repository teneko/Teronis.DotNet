using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Teronis.Data.TreeColumn;
using Teronis.Tools.Wpf;

namespace Teronis.AttachedProperties
{
    public static class ListViewTreeColumns
    {
        public static readonly DependencyProperty AppliableValuesProperty
            = DependencyProperty.RegisterAttached("AppliableValues", typeof(IEnumerable<ListViewTreeColumnValue>), typeof(ListViewTreeColumns), new PropertyMetadata(AppliableValuesChanged));

        public static void AppliableValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView) {
                var value = e.OldValue ?? e.NewValue;
                var treeColumnValues = (IEnumerable<ListViewTreeColumnValue>)value;
                ListViewTreeColumnValuesTools.ApplyTreeColumnsOnListView(listView, treeColumnValues);
            }
        }

        public static IEnumerable<ListViewTreeColumnValue> GetAppliableValues(ListView target)
            => (IEnumerable<ListViewTreeColumnValue>)target.GetValue(AppliableValuesProperty);

        public static void SetAppliableValues(ListView target, IEnumerable<ListViewTreeColumnValue> value)
            => target.SetValue(AppliableValuesProperty, value);
    }
}
