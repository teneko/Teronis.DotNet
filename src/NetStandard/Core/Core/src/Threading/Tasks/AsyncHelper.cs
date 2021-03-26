// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Threading.Tasks
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory taskFactory =
            new TaskFactory(CancellationToken.None,
                            TaskCreationOptions.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.Default);

        public static TResult RunSynchronous<TResult>(Func<Task<TResult>> func)
        {
            return taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSynchronous(Func<Task> func)
        {
            taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
