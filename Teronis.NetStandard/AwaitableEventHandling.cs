using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis
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
    public class AwaitableEventHandling : AwaitableEventHandling<Singleton>
    {
    }
}
