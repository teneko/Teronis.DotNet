using System;

namespace Teronis.Data.TreeColumn.Core
{
    public interface ITreeColumnKey 
    {
        Type DeclarationType { get; }
        string VariableName { get; }
    }   
}
