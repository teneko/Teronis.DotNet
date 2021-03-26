// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.ObjectModel.TreeColumn.Core
{
    public interface ITreeColumnDefinitions<TreeColumnDefinitionKeyType, TreeColumnDefinitionType>
        where TreeColumnDefinitionKeyType : ITreeColumnKey
        where TreeColumnDefinitionType : ITreeColumnValue<TreeColumnDefinitionKeyType>
    {
        IDictionary<TreeColumnDefinitionKeyType, TreeColumnDefinitionType> TreeColumnDefinitionByKey { get; }
    }
}
