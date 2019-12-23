using System;

namespace Teronis.Collections.ObjectModel
{
    public class IndexShiftConditionEvaluatingEventArgs<TShiftCondition> : EventArgs
    {
        public TShiftCondition ShiftCondition { get; private set; }

        public IndexShiftConditionEvaluatingEventArgs(TShiftCondition shiftCondition)
            => ShiftCondition = shiftCondition;
    }
}
