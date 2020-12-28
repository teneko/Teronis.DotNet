using System;
using Teronis.ObjectModel.TreeColumn.Core;
using Teronis.Windows.PresentationFoundation.Templating;

namespace Teronis.Windows.PresentationFoundation.Data.TreeColumn
{
    public class ListViewTreeColumnSeeker : TreeColumnSeekerBase<ListViewTreeColumnKey, ListViewTreeColumnValue>
    {
        public ListViewTreeColumnSeeker(Type HasTreeColumnsType) : base(HasTreeColumnsType) { }

        protected override ListViewTreeColumnValue instantiateTreeColumnValue(ListViewTreeColumnKey key, string path, int index)
        {
            var binding = key.GetBinding?.Invoke(path) ?? new CultureAwareBinding(path, key.PropertyInfo.PropertyType);
            var value = new ListViewTreeColumnValue(key, binding, index);

            if (key.GetCellTemplate == null && key.GetCellTemplateSelector == null) {
                value.DataTemplate = new BoundTextBlockTemplate() {
                    TextPropertyBinding = binding
                };
            } else if (key.GetCellTemplate != null) {
                value.DataTemplate = key.GetCellTemplate(binding);
            } else {
                value.DataTemplateSelector = key.GetCellTemplateSelector?.Invoke(binding);
            }

            return value;
        }
    }
}
