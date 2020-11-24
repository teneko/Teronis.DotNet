using System.Collections.Generic;

namespace Teronis.DataModeling.TreeColumn.Core
{
    public class TreeColumnKeyEqualityComparer : EqualityComparer<TreeColumnKey>
    {
        public new static readonly TreeColumnKeyEqualityComparer Default = new TreeColumnKeyEqualityComparer();

        public override bool Equals(TreeColumnKey? x, TreeColumnKey? y)
        {
            if (x is null && y is null) {
                return true;
            } else if (x == null || y == null) {
                return false;
            } else {
                return x.DeclaringType == y.DeclaringType && x.VariableName == y.VariableName;
            }
        }

        public override int GetHashCode(TreeColumnKey obj)
        {
            unchecked {
                var hash = 17;
                hash = hash * 23 + obj.DeclaringType.GetHashCode();
                hash = hash * 23 + obj.VariableName.GetHashCode();
                return hash;
            }
        }
    }
}
