// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public interface ITreeIterator<TEntry>
    {
        IEnumerator<TEntry> Enumerator { get; }
        bool StopRequested { get; }
    }
}
