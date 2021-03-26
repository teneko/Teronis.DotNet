// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class WaitHandleExtensions
    {
        public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource();

            var registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, isTimedOut) => {
                var localTcs = (TaskCompletionSource)state!;

                if (isTimedOut) {
                    localTcs.TrySetCanceled();
                } else {
                    localTcs.TrySetResult();
                }
            }, tcs, timeout, executeOnlyOnce: true);

            return tcs.Task.ContinueWith((_, state) =>
                ((RegisteredWaitHandle)state!).Unregister(null), registration, TaskScheduler.Default);
        }

        public static Task AsTask(this WaitHandle handle)
            => AsTask(handle, Timeout.InfiniteTimeSpan);
    }
}
