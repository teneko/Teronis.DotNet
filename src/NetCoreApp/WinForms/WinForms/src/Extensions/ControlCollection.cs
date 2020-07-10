using System;
using System.Windows.Forms;

namespace Teronis.Extensions
{
    public static class ControlCollectionExtensions
    {
        // for ControlCollection
        public static bool Swap(this Control.ControlCollection source, int fromIndex, int toIndex)
        {
            void insertAtCallback(int index, object item)
            {
                source.Add((Control)item);
                source.SetChildIndex((Control)item, index);
            }

            return source.Swap(fromIndex, toIndex, insertAtCallback);
        }

        public static void Shift<T>(this Control.ControlCollection source, T from, T to, Func<T, bool> isItemMovable, Action<int, int> fromIndexToIndexAction = null, Action<T, T> fromToAction = null) where T : Control
        {
            var fromIndex = source.IndexOf(from);
            var toIndex = source.IndexOf(to);
            source.Shift(fromIndex, toIndex, isItemMovable, fromIndexToIndexAction, fromToAction);
        }

        public static void Shift<T>(this Control.ControlCollection source, int fromIndex, int toIndex, Func<T, bool> isItemMovable, Action<int, int> fromIndex_ToIndexCallback = null, Action<T, T> from_ToCallback = null) where T : Control
        {
            if (isItemMovable((T)source[fromIndex]) && isItemMovable((T)source[toIndex])) {
                var ascend = fromIndex < toIndex;

                for (; ; )
{
                    if (ascend && fromIndex < toIndex || !ascend && fromIndex > toIndex) {
                        var _toIndex = fromIndex;

                        next:
                        var _fromIndex = fromIndex + (ascend ? 1 : -1);
                        void next() => fromIndex = ascend ? fromIndex + 1 : fromIndex - 1;

                        if (_fromIndex == toIndex || _fromIndex != toIndex && isItemMovable((T)source[_fromIndex])) {
                            source.Swap(_fromIndex, _toIndex);
                            fromIndex_ToIndexCallback?.Invoke(_fromIndex, _toIndex);
                            from_ToCallback?.Invoke((T)source[_fromIndex], (T)source[_toIndex]);
                            next();
                        } else {
                            next();
                            goto next;
                        }
                    } else {
                        break;
                    }
                }
            }
        }
    }
}
