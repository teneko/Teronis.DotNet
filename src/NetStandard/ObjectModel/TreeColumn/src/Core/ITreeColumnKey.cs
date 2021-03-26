// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.ObjectModel.TreeColumn.Core
{
    public interface ITreeColumnKey
    {
        Type DeclaringType { get; }
        string VariableName { get; }
    }
}
