// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teronis.Threading.Tasks
{
    /// <summary>
    /// This class can coordinate the invocation order of async events.
    /// <para/>
    /// In begin of the event method you may use one of the following methods:
    /// [<see cref="RegisterDependency(KeyType)"/> or <see cref="RegisterDependency(KeyType, out TaskCompletionSource)"/>], and [<see cref="TryAwaitDependency(KeyType[])"/>].
    /// <para/>
    /// After the event handler invocation:
    /// [<see cref="FinishDependenciesAsync"/>].
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public class AsyncEventSequence<KeyType>
        where KeyType : notnull
    {
        public AsyncEventSequenceStatus Status { get; private set; }
        public IEqualityComparer<KeyType> EqualityComparer { get; protected set; }
        public bool IsDisposed { get; private set; }

        private readonly Dictionary<KeyType, List<TaskCompletionSource>> tcsDependencies;
        private readonly TaskCompletionSource tcsRegistrationPhaseEnd;
        private Task? finishDependenciesTask;

        public AsyncEventSequence(IEqualityComparer<KeyType> equalityComparer)
        {
            Status = AsyncEventSequenceStatus.Created;
            EqualityComparer = equalityComparer ?? throw new ArgumentException(nameof(equalityComparer));
            tcsDependencies = new Dictionary<KeyType, List<TaskCompletionSource>>(EqualityComparer);
            tcsRegistrationPhaseEnd = new TaskCompletionSource();
        }

        public AsyncEventSequence()
            : this(EqualityComparer<KeyType>.Default) { }

        /// <summary>
        /// Checks if this instance is disposed. If true an exception
        /// of type <see cref="ObjectDisposedException"/> will be thrown.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Instance has been already disposed.</exception>
        private void checkDispose()
        {
            if (IsDisposed) {
                throw new ObjectDisposedException(null, "Object has been already disposed");
            }
        }

        public TaskCompletionSource RegisterDependency(KeyType key)
        {
            if (Status != AsyncEventSequenceStatus.Created) {
                throw new InvalidOperationException("Depenendecies are already awaited or are being awaited");
            }

            if (!tcsDependencies.TryGetValue(key, out var tcsList)) {
                tcsList = new List<TaskCompletionSource>();
                tcsDependencies.Add(key, tcsList);
            }

            var tcs = new TaskCompletionSource();
            tcsList.Add(tcs);
            return tcs;
        }

        public void RegisterDependency(KeyType key, out TaskCompletionSource rcs)
            => rcs = RegisterDependency(key);

        private IEnumerable<TaskCompletionSource> getAllTaskCompletionSources()
            => tcsDependencies.Values.SelectMany(x => x);

        /// <summary>
        /// This method awaits all dependencies selected by <paramref name="keys"/>. 
        /// If one of the awaiting dependencies are failing false gets returned. Even 
        /// when none keys are provided, this function awaits the task, that gets finished
        /// when dependency registration has been ended.
        /// </summary>
        public async Task<bool> TryAwaitDependency(params KeyType[] keys)
        {
            if (Status == AsyncEventSequenceStatus.Finished) {
                return true;
            } else if (Status == AsyncEventSequenceStatus.Canceled) {
                return false;
            } else {
                checkDispose();
                // We want to wait for the end of the registration, so that we can assure
                // that everyone could register their dependencies.
                await tcsRegistrationPhaseEnd.Task;
                IEnumerable<Task> awaitableDependencies;

                if (!(keys == null || keys.Length == 0)) {
                    // These are all dependencies that are referenced to passed keys
                    awaitableDependencies = from tcs in (from key in keys
                                                         where tcsDependencies.ContainsKey(key)
                                                         select tcsDependencies[key]).SelectMany(x => x)
                                            select tcs.Task;

                    // Finally we want to add them to the awaitable tasks
                    //awaitableTasks.AddRange(awaitableDependencies);
                } else {
                    awaitableDependencies = Enumerable.Empty<Task>();
                }

                try {
                    await Task.WhenAll(awaitableDependencies).ConfigureAwait(false);
                    return true;
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// This method awaits all registered dependencies and throws the first 
        /// occuring exception. You may call this after the event handler invocation.
        /// </summary>
        /// <exception cref="TaskCanceledException">Throws the first occuring exception when awaiting the dependencies.</exception>
        public async Task FinishDependenciesAsync()
        {
            lock (tcsRegistrationPhaseEnd) {
                if (finishDependenciesTask == null) {
                    // We want to finish the registration phase, after all invoked event handler may have registered their dependencies
                    tcsRegistrationPhaseEnd.SetResult();

                    var tasks = getAllTaskCompletionSources()
                        .Select(x => x.Task);

                    finishDependenciesTask = Task.WhenAll(tasks);
                    Status = AsyncEventSequenceStatus.Running;
                }
            }

            try {
                // Then we await all dependencies
                await finishDependenciesTask;
                Status = AsyncEventSequenceStatus.Finished;
            } catch {
                // Try to cancel all dependencies
                foreach (var tcs in getAllTaskCompletionSources()) {
                    tcs.TrySetCanceled();
                }

                Status = AsyncEventSequenceStatus.Canceled;
                throw;
            }

            await finishDependenciesTask;
        }
    }
}
