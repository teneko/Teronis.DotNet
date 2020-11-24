using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.DataModeling.TreeColumn.Core;
using Teronis.Reflection;

namespace Teronis.Utils
{
    public static class ITreeColumnValueUtils
    {
        public static bool DoesAnyColumnContainText(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumns, object entity, string searchText)
        {
            return treeColumns.Any(x => {
                if (x is null) {
                    return false;
                }

                var nestedPropertyValue = NestedPropertyUtils.GetNestedValue(entity, x.Path)?.ToString();

                if (nestedPropertyValue is null) {
                    return false;
                }

                return nestedPropertyValue.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0;
            });
        }

        public static IEnumerable<object?> GetCellContent(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumnValues, object cellContentContainer)
        {
            foreach (var treeColumnValue in treeColumnValues) {
                yield return NestedPropertyUtils.GetNestedValue(cellContentContainer, treeColumnValue.Path);
            }
        }
    }
}
