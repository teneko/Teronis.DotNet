using System;

namespace Teronis.DataModel.TreeColumn.Core
{
    public interface ITreeColumnKey
    {
        Type DeclaringType { get; }
        string VariableName { get; }
    }
}
