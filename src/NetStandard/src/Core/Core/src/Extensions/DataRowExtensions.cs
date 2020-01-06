using System.Collections;
using System.Data;
using System.Linq;

namespace Teronis.Extensions
{
    public static class DataRowExtensions
    {
        /* TODO: (re-)implement */
        //public static byte[] GenerateChecksum(this DataRow dataRow)
        //{
        //    for (var index = 0; index < dataRow.ItemArray.Length; index++)
        //        dataRow.ItemArray[index].GetType().FullName.ToConsole();

        //    return null;
        //}

        public static Hashtable GetHashtable(this DataRow row, string prefix = null)
        {
            var columns = row.Table.Columns;
            var ht = new Hashtable();

            foreach (DataColumn column in columns)
                ht[prefix + column.ColumnName] = row[column.ColumnName];

            return ht;
        }

        public static Hashtable GetHashtable(this DataRow row) => row.GetHashtable();

        public static DataRow CopyRow(this DataRow row)
        {
            var newRow = row.Table.NewRow();
            newRow.ItemArray = row.ItemArray;
            return newRow;
        }

        /* TODO: reimplement */
        //public static Hashtable GetOnlyChanges(this Hashtable row, Hashtable changes)
        //{
        //    var difference = new Hashtable();

        //    if (!(changes == null || changes.Count == 0)) {
        //        foreach (string key in changes.Keys) {
        //            var val = changes[key];

        //            if (!row[key].Equals(changes[key] == null ? DBNull.Value : (changes[key].ToString() == "" ? DBNull.Value : changes[key])))
        //                difference[key] = changes[key];
        //        }
        //    }

        //    return difference;
        //}

        //public static Hashtable GetOnlyChanges(this DataRow row, Hashtable changes)
        //{
        //    var difference = new Hashtable();

        //    if (!(changes == null || changes.Count == 0)) {
        //        foreach (string key in changes.Keys) {
        //            var val = changes[key];
        //            var dbaseVal = changes[key] == null ? DBNull.Value : (changes[key].ToString() == "" ? DBNull.Value : changes[key]);

        //            if (!row[key].Equals(dbaseVal))
        //                try {
        //                    if (Convert.IsDBNull(dbaseVal))
        //                        difference[key] = dbaseVal;
        //                    else
        //                        difference[key] = Convert.ChangeType(changes[key].ToString(), row.Table.Columns[key].DataType);
        //                } catch (Exception error) {
        //                    throw new Exception(($"{key}: {error.Message}"));
        //                }
        //        }
        //    }

        //    return difference;
        //}

        public static DataRow RightToLeft(this DataRow left, Hashtable right)
        {
            if (!(right == null || right.Count == 0))
                foreach (string key in right.Keys)
                    left[key] = right[key];

            return left;
        }

        public static Hashtable RightToLeft(this Hashtable left, Hashtable right, bool addNewKeys = false)
        {
            var leftKeys = left.Keys.Cast<string>();

            foreach (string key in right.Keys) {
                // Continue if new keys are not wished to be added to left.
                if (!addNewKeys && leftKeys.Contains(key))
                    continue;

                left[key] = right[key];
            }

            return left;
        }

        public static Hashtable LeftWithoutRight(this Hashtable left, Hashtable right)
        {
            foreach (var key in right.Keys)
                left.Remove(key);

            return left;
        }
    }
}
