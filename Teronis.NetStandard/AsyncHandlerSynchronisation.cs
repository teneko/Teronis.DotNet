using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teronis
{
    /// <summary>
    /// This class can coordinate the invocation order of async events. The procedure can look like this:
    /// <para/>
    /// In the begin of the event method you should use one of the following methods:
    /// [<see cref="RegisterDependency(KeyType)"/>|<see cref="RegisterDependency(KeyType, out TaskCompletionSource)"/>],
    /// [<see cref="AwaitDependency(KeyType)"/>].
    /// <para/>
    /// And in the end of the event handler method:
    /// <see cref="FinishRegistrationPhase"/>,
    /// [<see cref="AwaitAllDependencies"/>].
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public class AsyncHandlerSynchronization<KeyType>
    {
        private Dictionary<KeyType, List<TaskCompletionSource>> tcsDependencies;
        private TaskCompletionSource tcsRegistrationPhaseEnd;

        public AsyncHandlerSynchronization()
        {
            tcsDependencies = new Dictionary<KeyType, List<TaskCompletionSource>>();
            tcsRegistrationPhaseEnd = new TaskCompletionSource();
        }

        protected virtual IEqualityComparer<KeyType> GetComparer() => EqualityComparer<KeyType>.Default;

        public void RegisterDependency(KeyType key, out TaskCompletionSource rcs) => rcs = RegisterDependency(key);

        public TaskCompletionSource RegisterDependency(KeyType key)
        {
            if (!tcsDependencies.TryGetValue(key, out List<TaskCompletionSource> tcsList)) {
                tcsList = new List<TaskCompletionSource>();
                tcsDependencies.Add(key, tcsList);
            }

            var tcs = new TaskCompletionSource();
            tcsList.Add(tcs);
            return tcs;
        }

        public async Task AwaitRegistrationPhase() => await tcsRegistrationPhaseEnd.Task;

        /// <summary>
        /// This method automatically cancels all available <see cref="TaskCompletionSource"/>s if an exception by any awaiting dependency has been thrown.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<bool> TryAwaitDependency(params KeyType[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var tasks = from tcs in (from key in keys
                                     where tcsDependencies.ContainsKey(key)
                                     select tcsDependencies[key]).SelectMany(x => x)
                        select tcs.Task;

            try {
                await Task.WhenAll(tasks);
                return true;
            } catch {
                foreach (var tcs in getAllTaskCompletionSources())
                    tcs.TrySetCanceled();

                return false;
            }
        }

        public void FinishRegistrationPhase() => tcsRegistrationPhaseEnd.SetResult();

        /// <summary>
        /// You may call this after the event handler invocation. This method awaits all registered dependencies and can throw the first occuring exception.
        /// </summary>
        public async Task AwaitAllDependencies() => await Task.WhenAll(getAllTaskCompletionSources().Select(x => x.Task));

        private IEnumerable<TaskCompletionSource> getAllTaskCompletionSources() => tcsDependencies.Values.SelectMany(x => x);
    }
}
