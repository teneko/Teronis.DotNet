using System;

namespace Teronis.Collections.ObjectModel
{
    public class IndexShifter<TShiftCondition> : IDisposable
    {
        public event EventHandler<IndexShiftConditionEvaluatingEventArgs<TShiftCondition>> IndexShiftConditionEvaluating;

        private bool isDisposed;

        public void Shift(TShiftCondition condition)
        {
            var args = new IndexShiftConditionEvaluatingEventArgs<TShiftCondition>(condition);
            IndexShiftConditionEvaluating?.Invoke(this, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            IndexShiftConditionEvaluating = null;
            isDisposed = true;
        }

        ~IndexShifter() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
