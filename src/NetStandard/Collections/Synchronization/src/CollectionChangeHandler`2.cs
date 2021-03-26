// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    public class CollectionChangeHandler<ItemType>
    {
        public interface IDecoupledHandler
        {
            bool CanReplaceItem { get; }

            void InsertItem(IList<ItemType> list, int insertAt, ItemType item);
            void MoveItems(IList<ItemType> list, int fromIndex, int toIndex, int count);
            void RemoveItem(IList<ItemType> list, int removeAt);
            void ReplaceItem(IList<ItemType> list, int replaceAt, Func<ItemType> getItem);
            void Reset(IList<ItemType> list);
        }

        public class DecoupledHandler : IDecoupledHandler
        {
            public static DecoupledHandler Default = new DecoupledHandler();

            public virtual bool CanReplaceItem { get; } = false;

            public virtual void InsertItem(IList<ItemType> list, int insertAt, ItemType item) =>
                list.Insert(insertAt, item);

            public virtual void RemoveItem(IList<ItemType> list, int removeAt) =>
                list.RemoveAt(removeAt);

            public virtual void MoveItems(IList<ItemType> list, int fromIndex, int toIndex, int count) =>
                list.Move(fromIndex, toIndex, count);

            public virtual void ReplaceItem(IList<ItemType> list, int replaceAt, Func<ItemType> superItem)
            { }

            public virtual void Reset(IList<ItemType> list) =>
                list.Clear();
        }

        public class DecoupledItemReplacingHandler : DecoupledHandler {
            public new static DecoupledItemReplacingHandler Default = new DecoupledItemReplacingHandler();

            public override bool CanReplaceItem => true;

            public override void ReplaceItem(IList<ItemType> list, int replaceAt, Func<ItemType> getItem) =>
                list[replaceAt] = getItem();
        }

        public interface IDependencyInjectedHandler
        {
            IList<ItemType> Items { get; }
            bool CanReplaceItem { get; }

            void InsertItem(int insertAt, ItemType item);
            void MoveItems(int fromIndex, int toIndex, int count);
            void RemoveItem(int removeAt);
            void ReplaceItem(int replaceAt, Func<ItemType> superItem);
            void Reset();
        }

        public class DependencyInjectedHandler : IDependencyInjectedHandler
        {
            public virtual IList<ItemType> Items { get; } = null!;
            public virtual IDecoupledHandler Handler { get; } = null!;

            protected DependencyInjectedHandler() { }

            public DependencyInjectedHandler(IList<ItemType> items)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
                Handler = DecoupledHandler.Default;
            }

            public DependencyInjectedHandler(IList<ItemType> items, IDecoupledHandler? handler)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
                Handler = handler ?? DecoupledHandler.Default;
            }

            public virtual bool CanReplaceItem => Handler.CanReplaceItem;

            public virtual void InsertItem(int insertAt, ItemType item) =>
                Handler.InsertItem(Items, insertAt, item);

            public virtual void RemoveItem(int removeAt) =>
                Handler.RemoveItem(Items, removeAt);

            public virtual void MoveItems(int fromIndex, int toIndex, int count) =>
                Handler.MoveItems(Items, fromIndex, toIndex, count);

            public virtual void ReplaceItem(int replaceAt, Func<ItemType> getItem) =>
                Handler.ReplaceItem(Items, replaceAt, getItem);

            public virtual void Reset() =>
                Items.Clear();
        }
    }
}
