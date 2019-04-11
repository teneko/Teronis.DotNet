using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Data.TreeColumn.Core;

namespace Teronis.Tools.NetStandard
{
    public static class TreeColumnValuesTools
    {
        public static bool DoesAnyColumnContainText(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumns, object entity, string searchText)
            => treeColumns.Any(x => x == null ? false : ObjectTools.GetNestedValue(entity, x.Path)?.ToString().IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0);

        public static IEnumerable<object> GetCellContent(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumnValues, object cellContentContainer)
        {
            foreach (var treeColumnValue in treeColumnValues)
                yield return ObjectTools.GetNestedValue(cellContentContainer, treeColumnValue.Path);
        }
    }
}
