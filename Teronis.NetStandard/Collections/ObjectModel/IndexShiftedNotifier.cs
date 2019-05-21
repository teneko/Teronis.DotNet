using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections.ObjectModel
{
    public class IndexShiftedNotifier : IDisposable
    {
        public event EventHandler<IndexShiftedEventArgs> IndexShifted;

        private bool isDisposed;

        public void ShiftIndex(ICollectionChange<object> collectionChange)
        {
            var args = new IndexShiftedEventArgs(collectionChange);
            IndexShifted?.Invoke(this, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            IndexShifted = null;
            isDisposed = true;
        }

        ~IndexShiftedNotifier() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
