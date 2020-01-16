using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Data.TreeColumn.Core;
using Teronis.Reflection;

namespace Teronis.Tools
{
    public static class ITreeColumnValueTools
    {
        public static bool DoesAnyColumnContainText(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumns, object entity, string searchText)
            => treeColumns.Any(x => x == null ? false : NestedPropertyTools.GetNestedValue(entity, x.Path)?.ToString().IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0);

        public static IEnumerable<object> GetCellContent(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumnValues, object cellContentContainer)
        {
            foreach (var treeColumnValue in treeColumnValues)
                yield return NestedPropertyTools.GetNestedValue(cellContentContainer, treeColumnValue.Path);
        }
    }
}
