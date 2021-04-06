// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    public class CollectionChangeHandler<TItem>
    {
        public interface IDecoupledHandler
        {
            bool CanReplaceItem { get; }

            void InsertItem(IList<TItem> list, int insertAt, TItem item);
            void MoveItems(IList<TItem> list, int fromIndex, int toIndex, int count);
            void RemoveItem(IList<TItem> list, int removeAt);
            void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> getItem);
            void Reset(IList<TItem> list);
        }

        public class DecoupledHandler : IDecoupledHandler
        {
            public static DecoupledHandler Default = new DecoupledHandler();

            public virtual bool CanReplaceItem { get; } = false;

            public virtual void InsertItem(IList<TItem> list, int insertAt, TItem item) =>
                list.Insert(insertAt, item);

            public virtual void RemoveItem(IList<TItem> list, int removeAt) =>
                list.RemoveAt(removeAt);

            public virtual void MoveItems(IList<TItem> list, int fromIndex, int toIndex, int count) =>
                list.Move(fromIndex, toIndex, count);

            public virtual void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> superItem)
            { }

            public virtual void Reset(IList<TItem> list) =>
                list.Clear();
        }

        public class DecoupledItemReplacingHandler : DecoupledHandler {
            public new static DecoupledItemReplacingHandler Default = new DecoupledItemReplacingHandler();

            public override bool CanReplaceItem => true;

            public override void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> getItem) =>
                list[replaceAt] = getItem();
        }

        public interface IDependencyInjectedHandler
        {
            IList<TItem> Items { get; }
            bool CanReplaceItem { get; }

            void InsertItem(int insertAt, TItem item);
            void MoveItems(int fromIndex, int toIndex, int count);
            void RemoveItem(int removeAt);
            void ReplaceItem(int replaceAt, Func<TItem> superItem);
            void Reset();
        }

        public class DependencyInjectedHandler : IDependencyInjectedHandler
        {
            public virtual IList<TItem> Items { get; } = null!;
            public virtual IDecoupledHandler Handler { get; } = null!;

            protected DependencyInjectedHandler() { }

            public DependencyInjectedHandler(IList<TItem> items)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
                Handler = DecoupledHandler.Default;
            }

            public DependencyInjectedHandler(IList<TItem> items, IDecoupledHandler? handler)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
                Handler = handler ?? DecoupledHandler.Default;
            }

            public virtual bool CanReplaceItem => Handler.CanReplaceItem;

            public virtual void InsertItem(int insertAt, TItem item) =>
                Handler.InsertItem(Items, insertAt, item);

            public virtual void RemoveItem(int removeAt) =>
                Handler.RemoveItem(Items, removeAt);

            public virtual void MoveItems(int fromIndex, int toIndex, int count) =>
                Handler.MoveItems(Items, fromIndex, toIndex, count);

            public virtual void ReplaceItem(int replaceAt, Func<TItem> getItem) =>
                Handler.ReplaceItem(Items, replaceAt, getItem);

            public virtual void Reset() =>
                Items.Clear();
        }
    }
}
