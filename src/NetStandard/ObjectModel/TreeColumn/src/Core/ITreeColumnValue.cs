// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Teronis.ObjectModel.TreeColumn.Core
{
    public interface ITreeColumnValue<out TreeColumnDefinitionKeyType>
        where TreeColumnDefinitionKeyType : ITreeColumnKey
    {
        TreeColumnDefinitionKeyType Key { get; }
        string Path { get; }
        int Index { get; }
    }
}
