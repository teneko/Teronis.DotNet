using System.Collections.Generic;

namespace Teronis.Data.TreeColumn.Core
{
    public class TreeColumnKeyEqualityComparer : EqualityComparer<TreeColumnKey>
    {
        public static readonly TreeColumnKeyEqualityComparer Instance = new TreeColumnKeyEqualityComparer();

        public override bool Equals(TreeColumnKey x, TreeColumnKey y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.DeclarationType == y.DeclarationType && x.VariableName == y.VariableName;
        }

        public override int GetHashCode(TreeColumnKey obj)
        {
            unchecked {
                var hash = 17;
                hash = hash * 23 + obj.DeclarationType.GetHashCode();
                hash = hash * 23 + obj.VariableName.GetHashCode();
                return hash;
            }
        }
    }
}
