using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections.ObjectModel
{
    public class IndexShiftConditionEvaluatingEventArgs<TShiftCondition> : EventArgs
    {
        public TShiftCondition ShiftCondition { get; private set; }

        public IndexShiftConditionEvaluatingEventArgs(TShiftCondition shiftCondition)
            => ShiftCondition = shiftCondition;
    }
}
