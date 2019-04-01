using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.NetStandard
{
    public class CollectionChange<T>
    {
        public static CollectionChange<T> CreateReplaceChange(T leftValue, int leftValueIndex, T rightValue, int rightValueIndex) => new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, rightValueIndex);

        public NotifyCollectionChangedAction ChangeAction { get; private set; }
        public T LeftValue { get; private set; }
        public T RightValue { get; private set; }
        public int LeftIndex { get; private set; }
        public int RightIndex { get; private set; }

        public CollectionChange(NotifyCollectionChangedAction changeAction, T leftValue, int leftIndex, T rightValue, int rightIndex)
        {
            ChangeAction = changeAction;
            LeftValue = leftValue;
            RightValue = rightValue;
            LeftIndex = leftIndex;
            RightIndex = rightIndex;
        }

        public CollectionChange(NotifyCollectionChangedAction changeAction, T leftValue, int leftIndex) : this(changeAction, leftValue, leftIndex, default, default) { }
    }
}
