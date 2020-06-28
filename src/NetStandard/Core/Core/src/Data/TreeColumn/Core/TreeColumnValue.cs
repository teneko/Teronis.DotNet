

namespace Teronis.Data.TreeColumn.Core
{
    public class TreeColumnValue<TreeColumnDefinitionKeyType> : ITreeColumnValue<TreeColumnDefinitionKeyType>
        where TreeColumnDefinitionKeyType : ITreeColumnKey
    {
        public TreeColumnDefinitionKeyType Key { get; protected set; }
        public virtual string Path { get; protected set; }
        public virtual int Index { get; protected set; }

        public TreeColumnValue(TreeColumnDefinitionKeyType key, string path, int index)
        {
            Key = key;
            Path = path;
            Index = index;
        }
    }
}
