// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Teronis.Collections.DataSources.Generic
{
    public class ParallelDataSources<DataType> : AsyncDataSource<DataType>
    {
        private readonly IEnumerable<IAsyncDataSource<DataType>> asyncDataSources;
        //private CancellationTokenSource asyncDataSourcesCancellationTokenSource;

        public ParallelDataSources(IEnumerable<IAsyncDataSource<DataType>> asyncDataSources, ILogger? logger = null)
            : base(logger)
            => this.asyncDataSources = asyncDataSources ?? throw new ArgumentNullException(nameof(asyncDataSources));

        protected override async IAsyncEnumerable<DataType> EnumerateAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            yield return default!;

            var asyncEnumerators = asyncDataSources.Select(x => {
                var enumerationCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                RegisterCancellationTokenSource(enumerationCancellationTokenSource);
                return x.EnumerateAsync(cancellationToken).GetAsyncEnumerator(enumerationCancellationTokenSource.Token);
            });

            var dictionary = new Dictionary<Task, IAsyncEnumerator<DataType>>();

            foreach (var asyncEnumerator in asyncEnumerators) {
                dictionary.Add(asyncEnumerator.MoveNextAsync().AsTask(), asyncEnumerator);
            }

            try {
                //entry:
                //    cancellationToken.ThrowIfCancellationRequested();


            } finally {
                //await Task.WhenAll(dictionary.Values.Select(x => Dispose))
            }
        }
    }
}
