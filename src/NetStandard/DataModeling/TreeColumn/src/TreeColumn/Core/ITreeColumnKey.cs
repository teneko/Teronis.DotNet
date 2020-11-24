using System;

namespace Teronis.DataModeling.TreeColumn.Core
{
    public interface ITreeColumnKey
    {
        Type DeclaringType { get; }
        string VariableName { get; }
    }
}
