using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Teronis.Windows.PresentationFoundation.Utils;

namespace Teronis.Windows.PresentationFoundation.Controls
{
    public static class ListBoxUtils
    {
        private static ListViewItem getListViewItem(ListBox listBox, int index)
        {
            if (listBox.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) {
                return null;
            }

            return listBox.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        public static int GetSelectedItemIndexAtPosition(ListBox listBox, GetPositionFromInputElementDelegate getPosition)
        {
            var index = -1;

            if (listBox?.Items != null) {
                for (int i = 0; i < listBox.Items.Count; ++i) {
                    if (listBox.SelectedItems.IndexOf(listBox.Items[i]) == -1) {
                        continue;
                    }

                    var listViewItem = getListViewItem(listBox, i);

                    if (VisualTreeUtils.IsMouseOverTarget(listViewItem, getPosition)) {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        /// <summary>
        /// The list will never be null but empty.
        /// </summary>
        public static List<T> GetSelectedItemsAtPosition<T>(ListBox listBox, GetPositionFromInputElementDelegate getPosition)
        {
            var selectedItems = new List<T>();

            var itemIndexAtPosition = GetSelectedItemIndexAtPosition(listBox, getPosition);

            // When mouse is over one item that is selected then we can return all selected items.
            if (itemIndexAtPosition >= 0) {
                selectedItems.AddRange(listBox.SelectedItems.OfType<T>());
            }

            return selectedItems;
        }
    }
}
