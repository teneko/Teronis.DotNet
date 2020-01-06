using System;
using System.Reflection;

namespace Teronis.Data.TreeColumn.Core
{
    public class TreeColumnKey : ITreeColumnKey
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public Type DeclaringType => PropertyInfo.DeclaringType;
        public string VariableName => PropertyInfo.Name;

        public TreeColumnKey(Type declarationType, string variableName)
            => PropertyInfo = declarationType.GetProperty(variableName);

        public bool Equals(TreeColumnKey other) => TreeColumnKeyEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as TreeColumnKey);
        public override int GetHashCode() => TreeColumnKeyEqualityComparer.Default.GetHashCode(this);
    }
}
