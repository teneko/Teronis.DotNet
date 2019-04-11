using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChange<T>
    {
        public static CollectionChange<T> CreateLeft(NotifyCollectionChangedAction changeAction, T leftValue, int leftIndex)
            => new CollectionChange<T>(changeAction, leftValue, leftIndex, default, -1);

        public static CollectionChange<T> CreateRight(NotifyCollectionChangedAction changeAction, T rightValue, int rightValueIndex)
            => new CollectionChange<T>(changeAction, default, -1, rightValue, rightValueIndex);

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
    }
}
