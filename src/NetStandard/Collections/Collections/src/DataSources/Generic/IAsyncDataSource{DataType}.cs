// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

namespace Teronis.Collections.DataSources.Generic
{
    public interface IAsyncDataSource<out DataType> : IDataSource<DataType>
    {
        IAsyncEnumerable<DataType> EnumerateAsync(CancellationToken cancellationToken = default);
    }
}
