using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public class ItemComparison<T>
    {
        public T LeftValue { get; private set; }
        public ECompareOperator CompareOperator { get; private set; }
        /// <summary>
        /// Not default/null if <see cref="ECompareOperator"/> is equals <see cref="ECompareOperator.Update"/>.
        /// </summary>
        public T RightValue { get; private set; }

        public ItemComparison(T leftValue, ECompareOperator compareOperator, T rightValue)
        {
            LeftValue = leftValue;
            CompareOperator = compareOperator;
            RightValue = rightValue;
        }


        public ItemComparison(T originValue, ECompareOperator differenceType) : this(originValue, differenceType, default) { }
        public ItemComparison(ECompareOperator compareOperator, T rightValue) : this(default, compareOperator, rightValue) { }
    }
}
