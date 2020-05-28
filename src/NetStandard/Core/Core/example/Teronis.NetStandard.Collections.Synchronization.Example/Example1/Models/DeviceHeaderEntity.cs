using System.Linq;
using System.Collections.Generic;
using Teronis.Data.TreeColumn;
using Teronis.Data.TreeColumn.Core;
using Teronis.Tools;
using System;
using Teronis.Extensions;
using System.Diagnostics;
using Teronis.Diagnostics;
using Teronis.Reflection;

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

        public bool DoesEachColumnContainsText(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumns, string searchText)
           => treeColumns.Any(x => x == null ? false : NestedPropertyTools.GetNestedValue(this, x.Path)?.ToString().IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0);

        public IEnumerable<object> GetCellContents(IEnumerable<ITreeColumnValue<ITreeColumnKey>> treeColumnValues)
            => ITreeColumnValueTools.GetCellContent(treeColumnValues, this);
    }
}
