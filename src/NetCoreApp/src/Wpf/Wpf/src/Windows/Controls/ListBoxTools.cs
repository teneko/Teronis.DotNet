using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Teronis.Windows.Controls
{
    public static class ListBoxTools
    {
        private static ListViewItem getListViewItem(ListBox listBox, int index)
        {
            if (listBox.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return listBox.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        public static int GetSelectedItemIndexAtPosition(ListBox listBox, GetPositionFromInputElementDelegate getPosition)
        {
            var index = -1;

            if (listBox?.Items != null) {
                for (int i = 0; i < listBox.Items.Count; ++i) {
                    if (listBox.SelectedItems.IndexOf(listBox.Items[i]) == -1)
                        continue;

                    var listViewItem = getListViewItem(listBox, i);

                    if (VisualTools.IsMouseOverTarget(listViewItem, getPosition)) {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }
    }
}
