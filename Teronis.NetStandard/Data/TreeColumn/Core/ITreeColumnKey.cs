using System;

namespace Teronis.Data.TreeColumn.Core
{
    public interface ITreeColumnKey 
    {
        Type DeclaringType { get; }
        string VariableName { get; }
    }   
}
