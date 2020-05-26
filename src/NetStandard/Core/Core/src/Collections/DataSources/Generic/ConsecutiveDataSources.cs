using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Teronis.Collections.DataSources.Generic
{
    public class ConsecutiveDataSources<DataType> : AsyncDataSource<DataType>
    {
        private IEnumerable<IAsyncDataSource<DataType>> asyncDataSources;

        public ConsecutiveDataSources(IEnumerable<IAsyncDataSource<DataType>> asyncDataSources, ILogger logger)
            : base(logger)
            => this.asyncDataSources = asyncDataSources ?? throw new ArgumentNullException(nameof(asyncDataSources));

        protected override async IAsyncEnumerable<DataType> EnumerateAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var currentAsyncDataSource in asyncDataSources)
                await foreach (var data in currentAsyncDataSource.EnumerateAsync(cancellationToken))
                    yield return data;
        }
    }
}
