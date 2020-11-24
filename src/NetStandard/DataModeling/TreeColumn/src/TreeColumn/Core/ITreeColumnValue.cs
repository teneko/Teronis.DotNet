

namespace Teronis.DataModeling.TreeColumn.Core
{
    public interface ITreeColumnValue<out TreeColumnDefinitionKeyType>
        where TreeColumnDefinitionKeyType : ITreeColumnKey
    {
        TreeColumnDefinitionKeyType Key { get; }
        string Path { get; }
        int Index { get; }
    }
}
