using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Teronis.DataModel.TreeColumn;
using Teronis.DataModel.TreeColumn.Core;
using Teronis.Diagnostics;
using Teronis.Extensions;
using Teronis.Reflection;
using Teronis.Tools;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    [DebuggerDisplay(IDebuggerDisplayLibrary.FullGetDebuggerDisplayMethodPathWithParameterizedThis)]
    public class DeviceHeaderEntity : Entity, IDebuggerDisplay
    {
        public string Serial { get; set; }
        public string SerialAlias { get; set; }
        public string ClientCode { get; set; }
        public string Language { get; set; }
        public int? Timezone { get; set; }

        [HasTreeColumns]
        public DeviceHeaderStateEntity State { get; set; }

        string IDebuggerDisplay.DebuggerDisplay => $"{GetType()}, {nameof(Serial)} = {Serial}";

        public bool DoesEachColumnContainsText(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumns, string searchText) =>
            treeColumns.Any(x => x != null
                && NestedPropertyTools.GetNestedValue(this, x.Path)?.ToString().IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0);

        public IEnumerable<object> GetCellContents(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumnValues) =>
            ITreeColumnValueTools.GetCellContent(treeColumnValues, this);
    }
}
