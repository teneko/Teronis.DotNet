using System;

namespace Teronis.Tools
{
    public static class CollectionTools
    {
        public static (int MinIndex, int Distance) GetMoveRange(int oldItemIndex, int newItemIndex, int itemsCount)
        {
            var amountOfAffectedItems = Math.Abs(oldItemIndex - newItemIndex) + itemsCount;
            var startIndexOfAffectedItems = Math.Min(oldItemIndex, newItemIndex);
            return (MinIndex: startIndexOfAffectedItems, Distance: amountOfAffectedItems);
        }

        public static bool MoveRangeContains(int oldItemIndex, int newItemIndex, int itemsCount, int itemIndex)
        {
            var (StartIndexOfAffectedItems, AmountOfAffectedItems) = GetMoveRange(oldItemIndex, newItemIndex, itemsCount);
            return itemIndex >= StartIndexOfAffectedItems && itemIndex < StartIndexOfAffectedItems + AmountOfAffectedItems;
        }
    }
}
