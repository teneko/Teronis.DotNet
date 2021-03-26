// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Teronis.Collections.DataSources.Generic
{
    public class EnumerableDataSource<DataType> : AsyncDataSource<DataType>
    {
        public static EnumerableDataSource<DataType> Create(IEnumerable<DataType> enumerable, ILogger logger)
        {
            enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
            var asyncEnumerable = enumerable.ToAsyncEnumerable();
            var dataSource = new EnumerableDataSource<DataType>(asyncEnumerable, logger);
            return dataSource;
        }

        private readonly IAsyncEnumerable<DataType> asyncEnumerable;

        public EnumerableDataSource(IAsyncEnumerable<DataType> asyncEnumerable, ILogger logger)
            : base(logger)
            => this.asyncEnumerable = asyncEnumerable ?? throw new ArgumentNullException(nameof(asyncEnumerable));

        protected override IAsyncEnumerable<DataType> EnumerateAsync(CancellationToken cancellationToken = default)
            => asyncEnumerable;
    }
}
