using System;

namespace Teronis.Data.TreeColumn.Core
{
    public class TreeColumnKey : ITreeColumnKey
    {
        public Type DeclarationType { get; private set; }
        public string VariableName { get; private set; }

        public TreeColumnKey(Type declarationType, string variableName)
        {
            DeclarationType = declarationType;
            VariableName = variableName;
        }

        public bool Equals(TreeColumnKey other) => TreeColumnKeyEqualityComparer.Instance.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as TreeColumnKey);
        public override int GetHashCode() => TreeColumnKeyEqualityComparer.Instance.GetHashCode(this);
    }
}
