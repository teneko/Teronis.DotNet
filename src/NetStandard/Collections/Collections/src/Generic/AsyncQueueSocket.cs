// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Teronis.Collections.Generic
{
    public class AsyncQueueSocket<T> : IAsyncEnumerable<T>, IDisposable
    {
        public bool IsDisposed { get; private set; }

        private readonly SemaphoreSlim enumerationSemaphore;
        private readonly BufferBlock<T> bufferBlock;

        public AsyncQueueSocket()
        {
            enumerationSemaphore = new SemaphoreSlim(1);
            bufferBlock = new BufferBlock<T>();
        }

        public void Enqueue(T item)
            => bufferBlock.Post(item);

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
        {
            // We lock this so we only ever enumerate once at a time.
            // That way we ensure all items are returned in a continuous
            // fashion with no 'holes' in the data when two foreach compete.
            using (enumerationSemaphore) {
                // Return new elements until cancellationToken is triggered.
                while (true) {
                    // Make sure to throw on cancellation so the Task will transfer into a canceled state
                    token.ThrowIfCancellationRequested();
                    yield return await bufferBlock.ReceiveAsync(token);
                }
            }
        }

        #region IDisposable Support

        public void Dispose()
            => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                IsDisposed = true;

                if (disposing) {
                    enumerationSemaphore.Dispose();
                }
            }
        }

        #endregion
    }
}
