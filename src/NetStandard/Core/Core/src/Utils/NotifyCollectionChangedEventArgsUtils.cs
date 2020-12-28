using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace Teronis.Utils
{
    public static class NotifyCollectionChangedEventArgsUtils
    {
        internal static Lazy<Action<object, object?>> LazyNotifyCollectionChangedEventArgsActionSetMethod;
        internal static Lazy<Action<object, object?>> LazyNotifyCollectionChangedEventArgsOldItemsSetMethod;
        internal static Lazy<Action<object, object?>> LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod;
        internal static Lazy<Action<object, object?>> LazyNotifyCollectionChangedEventArgsNewItemsSetMethod;
        internal static Lazy<Action<object, object?>> LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod;

        static NotifyCollectionChangedEventArgsUtils()
        {
            var notifyCollectionChangedEventArgsType = typeof(NotifyCollectionChangedEventArgs);
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            LazyNotifyCollectionChangedEventArgsActionSetMethod = new Lazy<Action<object, object?>>(() => 
                (Action<object, object?>)notifyCollectionChangedEventArgsType.GetField("_action", bindingFlags).SetValue);

            LazyNotifyCollectionChangedEventArgsOldItemsSetMethod = new Lazy<Action<object, object?>>(() =>
                 (Action<object, object?>)notifyCollectionChangedEventArgsType.GetField("_oldItems", bindingFlags).SetValue);

            LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod = new Lazy<Action<object, object?>>(() =>
                 (Action<object, object?>)notifyCollectionChangedEventArgsType.GetField("_oldStartingIndex", bindingFlags).SetValue);

            LazyNotifyCollectionChangedEventArgsNewItemsSetMethod = new Lazy<Action<object, object?>>(() =>
                 (Action<object, object?>)notifyCollectionChangedEventArgsType.GetField("_newItems", bindingFlags).SetValue);

            LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod = new Lazy<Action<object, object?>>(() =>
                 (Action<object, object?>)notifyCollectionChangedEventArgsType.GetField("_newStartingIndex", bindingFlags).SetValue);
        }

        public static void SetNotifyCollectionChangedEventArgsProperties(
           NotifyCollectionChangedEventArgs eventArgs,
           NotifyCollectionChangedAction action,
           IList? oldItems,
           int oldStartingIndex,
           IList? newItems,
           int newStartingIndex)
        {
            LazyNotifyCollectionChangedEventArgsActionSetMethod.Value(eventArgs, action);
            LazyNotifyCollectionChangedEventArgsOldItemsSetMethod.Value(eventArgs, oldItems);
            LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod.Value(eventArgs, oldStartingIndex);
            LazyNotifyCollectionChangedEventArgsNewItemsSetMethod.Value(eventArgs, newItems);
            LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod.Value(eventArgs, newStartingIndex);
        }

        public static NotifyCollectionChangedEventArgs CreateNotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList? oldItems,
            int oldStartingIndex,
            IList? newItems,
            int newStartingIndex)
        {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            SetNotifyCollectionChangedEventArgsProperties(eventArgs, action, oldItems, oldStartingIndex, newItems, newStartingIndex);
            return eventArgs;
        }
    }
}
