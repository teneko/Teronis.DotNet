using System;
using System.Reflection;

namespace Teronis.ObjectModel.TreeColumn.Core
{
    public class TreeColumnKey : ITreeColumnKey
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public Type DeclaringType => PropertyInfo.DeclaringType!;
        public string VariableName => PropertyInfo.Name;

        public TreeColumnKey(Type declarationType, string variableName)
        {
            declarationType = declarationType ?? throw new ArgumentNullException(nameof(declarationType));

            PropertyInfo = declarationType.GetProperty(variableName) ??
                throw new ArgumentException("Property does not exist.");

            if (PropertyInfo.DeclaringType == null) {
                throw new ArgumentException("Declaring type is null.");
            }
        }

        public bool Equals(TreeColumnKey other) =>
            TreeColumnKeyEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object? obj)
        {
            if (obj is TreeColumnKey treeColumnKey) {
                return Equals(treeColumnKey);
            }

            return false;
        }

        public override int GetHashCode() =>
            TreeColumnKeyEqualityComparer.Default.GetHashCode(this);
    }
}
