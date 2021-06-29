// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using Concurrent.FastReflection.NetStandard;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class NotifyCollectionChangedEventArgsUtils
    {
        internal static Lazy<MemberSetter<NotifyCollectionChangedEventArgs, NotifyCollectionChangedAction>> LazyNotifyCollectionChangedEventArgsActionSetMethod;
        internal static Lazy<MemberSetter<NotifyCollectionChangedEventArgs, IList?>> LazyNotifyCollectionChangedEventArgsOldItemsSetMethod;
        internal static Lazy<MemberSetter<NotifyCollectionChangedEventArgs, int>> LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod;
        internal static Lazy<MemberSetter<NotifyCollectionChangedEventArgs, IList?>> LazyNotifyCollectionChangedEventArgsNewItemsSetMethod;
        internal static Lazy<MemberSetter<NotifyCollectionChangedEventArgs, int>> LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod;

        static NotifyCollectionChangedEventArgsUtils()
        {
            var notifyCollectionChangedEventArgsType = typeof(NotifyCollectionChangedEventArgs);
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            LazyNotifyCollectionChangedEventArgsActionSetMethod = new Lazy<MemberSetter<NotifyCollectionChangedEventArgs, NotifyCollectionChangedAction>>(() =>
                notifyCollectionChangedEventArgsType.GetField("_action", bindingFlags)!.DelegateForSet<NotifyCollectionChangedEventArgs, NotifyCollectionChangedAction>());

            LazyNotifyCollectionChangedEventArgsOldItemsSetMethod = new Lazy<MemberSetter<NotifyCollectionChangedEventArgs, IList?>>(() =>
                notifyCollectionChangedEventArgsType.GetField("_oldItems", bindingFlags)!.DelegateForSet<NotifyCollectionChangedEventArgs,IList?>());

            LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod = new Lazy<MemberSetter<NotifyCollectionChangedEventArgs, int>>(() =>
                notifyCollectionChangedEventArgsType.GetField("_oldStartingIndex", bindingFlags)!.DelegateForSet<NotifyCollectionChangedEventArgs, int>());

            LazyNotifyCollectionChangedEventArgsNewItemsSetMethod = new Lazy<MemberSetter<NotifyCollectionChangedEventArgs, IList?>>(() =>
                notifyCollectionChangedEventArgsType.GetField("_newItems", bindingFlags)!.DelegateForSet<NotifyCollectionChangedEventArgs, IList?>());

            LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod = new Lazy<MemberSetter<NotifyCollectionChangedEventArgs, int>>(() =>
                notifyCollectionChangedEventArgsType.GetField("_newStartingIndex", bindingFlags)!.DelegateForSet<NotifyCollectionChangedEventArgs, int>());
        }

        public static void SetNotifyCollectionChangedEventArgsProperties(
           NotifyCollectionChangedEventArgs eventArgs,
           NotifyCollectionChangedAction action,
           IList? oldItems,
           int oldStartingIndex,
           IList? newItems,
           int newStartingIndex)
        {
            LazyNotifyCollectionChangedEventArgsActionSetMethod.Value(ref eventArgs, action);
            LazyNotifyCollectionChangedEventArgsOldItemsSetMethod.Value(ref eventArgs, oldItems);
            LazyNotifyCollectionChangedEventArgsOldStartingIndexSetMethod.Value(ref eventArgs, oldStartingIndex);
            LazyNotifyCollectionChangedEventArgsNewItemsSetMethod.Value(ref eventArgs, newItems);
            LazyNotifyCollectionChangedEventArgsNewStartingIndexSetMethod.Value(ref eventArgs, newStartingIndex);
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
