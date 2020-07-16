using System;
using System.Collections.Generic;
using Teronis.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.DataModel.TreeColumn.Core
{
    public abstract class TreeColumnSeekerBase<TreeColumnKeyType, TreeColumnValueType>
        where TreeColumnKeyType : ITreeColumnKey
        where TreeColumnValueType : ITreeColumnValue<TreeColumnKeyType>
    {
        public IDictionary<TreeColumnKey, ITreeColumnValue<TreeColumnKeyType>> TreeColumnDefinitionByKey { get; private set; }
        public Type TreeColumnsHolderType { get; private set; }

        public TreeColumnSeekerBase(Type treeColumnsHolderType)
        {
            TreeColumnDefinitionByKey = new Dictionary<TreeColumnKey, ITreeColumnValue<TreeColumnKeyType>>();
            TreeColumnsHolderType = treeColumnsHolderType;
        }

        protected abstract TreeColumnValueType instantiateTreeColumnValue(TreeColumnKeyType key, string path, int index);

        public IDictionary<TreeColumnKeyType, TreeColumnValueType> SearchTreeColumnDefinitions(IList<TreeColumnKeyType> treeColumnOrdering)
        {
            /// Cache for declaration paths of children that are decorated with <see cref="HasTreeColumnsAttribute"/>
            var columnDefinitionsByParent = new List<(Type DeclaringType, string? Path)>(new[] { (TreeColumnsHolderType, default(string)) });
            var treeColumnDefinitions = new OrderedDictionary<TreeColumnKeyType, TreeColumnValueType>();

            while (columnDefinitionsByParent.Count > 0) {
                static string combinePath(string? left, string right)
                {
                    string combinedPath;

                    if (left == null) {
                        combinedPath = right;
                    } else {
                        combinedPath = left + "." + right;
                    }

                    return combinedPath;
                }

                var (declaringType, parentPath) = columnDefinitionsByParent[0];

                // We cache declaration path children that are existing in the current declaration path
                foreach (var varInfo in declaringType.GetAttributePropertyMembers<HasTreeColumnsAttribute>()) {
                    var propertyName = varInfo.MemberInfo.Name;
                    string combinedPath = combinePath(parentPath, varInfo.MemberInfo.Name);
                    columnDefinitionsByParent.Add((varInfo.MemberInfo.GetVariableType(), combinedPath));
                }

                for (int index = 0; index < treeColumnOrdering.Count; index++) {
                    var orderedTreeColumnKey = treeColumnOrdering[index];

                    if (orderedTreeColumnKey.DeclaringType == declaringType) {
                        string combinedPath = combinePath(parentPath, orderedTreeColumnKey.VariableName);
                        var treeColumnValue = instantiateTreeColumnValue(orderedTreeColumnKey, combinedPath, index);

                        if (index < treeColumnDefinitions.Count) {
                            treeColumnDefinitions.Insert(index, orderedTreeColumnKey, treeColumnValue);
                        } else {
                            treeColumnDefinitions.Add(orderedTreeColumnKey, treeColumnValue);
                        }
                    }
                }

                columnDefinitionsByParent.RemoveAt(0);
            }

            return treeColumnDefinitions;
        }
    }
}
