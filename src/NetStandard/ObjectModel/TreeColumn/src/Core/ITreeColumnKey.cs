using System;

namespace Teronis.ObjectModel.TreeColumn.Core
{
    public interface ITreeColumnKey
    {
        Type DeclaringType { get; }
        string VariableName { get; }
    }
}
