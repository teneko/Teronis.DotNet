using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Teronis.Windows.PresentationFoundation.Controls;
using Teronis.Windows.PresentationFoundation.Data.TreeColumn;

namespace Teronis.Windows.PresentationFoundation.AttachedProperties
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
                ListViewUtils.ApplyTreeColumnsOnListView(listView, treeColumnValues);
            }
        }

        public static IEnumerable<ListViewTreeColumnValue> GetAppliableValues(ListView target)
            => (IEnumerable<ListViewTreeColumnValue>)target.GetValue(AppliableValuesProperty);

        public static void SetAppliableValues(ListView target, IEnumerable<ListViewTreeColumnValue> value)
            => target.SetValue(AppliableValuesProperty, value);
    }
}
