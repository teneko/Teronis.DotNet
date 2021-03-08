using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public readonly struct JSFacadePropertiesInitialization : IAsyncDisposable, IReadOnlyList<IAsyncDisposable>
    {
        public int Count =>
            referenceWrappers.Count;

        private readonly IReadOnlyList<IAsyncDisposable> referenceWrappers;

        public JSFacadePropertiesInitialization(IReadOnlyList<IAsyncDisposable> referenceWrappers) {
            this.referenceWrappers = referenceWrappers ?? throw new ArgumentNullException(nameof(referenceWrappers));
        }

        public IAsyncDisposable this[int index] =>
            referenceWrappers[index];

        public IEnumerator<IAsyncDisposable> GetEnumerator()
        {
            return referenceWrappers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)referenceWrappers).GetEnumerator();
        }

        public async ValueTask DisposeAsync()
        {
            if (referenceWrappers is null) {
                return;
            }

            foreach (var moduleWrapper in referenceWrappers) {
                await moduleWrapper.DisposeAsync();
            }
        }
    }
}
