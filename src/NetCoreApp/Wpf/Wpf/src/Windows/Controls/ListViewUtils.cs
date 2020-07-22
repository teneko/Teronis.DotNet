using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Teronis.Windows.Data.TreeColumn;
using Teronis.Wpf.AttachedProperties;

namespace Teronis.Windows.Controls
{
    public static class ListViewUtils
    {
        public static void ApplyTreeColumnsOnListView(ListView listView, IEnumerable<ListViewTreeColumnValue> treeColumns)
        {
            var gridView = new GridView();

            foreach (var columnDefinition in treeColumns) {
                var displayMemberBinding = new Binding(columnDefinition.Path);

                var gridViewColumn = new GridViewColumn() {
                    Header = columnDefinition.Key.Header
                };

                ListViewTreeColumn.SetBinding(gridViewColumn, displayMemberBinding);

                if (columnDefinition.DataTemplate != null) {
                    gridViewColumn.CellTemplate = columnDefinition.DataTemplate;
                } else {
                    gridViewColumn.CellTemplateSelector = columnDefinition.DataTemplateSelector;
                }

                gridView.Columns.Add(gridViewColumn);
            }

            listView.View = gridView;
        }
    }
}
