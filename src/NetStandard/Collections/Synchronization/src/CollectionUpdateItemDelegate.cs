using System;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// Used to update the item by new item. When the new item is not present at the time the modification is applying it will be
    /// created once and returned on subsequent calls.
    /// </summary>
    /// <typeparam name="ItemType"></typeparam>
    /// <typeparam name="NewItemType"></typeparam>
    /// <param name="item"></param>
    /// <param name="createOrGetNewItem">
    /// Creates or gets the new item from the modification.
    /// It gets created like the item that would be normally 
    /// created at first at insertion. It is in such a way designed
    /// to return the same reference as previously once called. It
    /// will be the same reference that may be created through 
    /// <see cref="CollectionChangeHandler{NewItemType}.IDependencyInjectedHandler.ReplaceItem(int, Func{NewItemType})"/>.
    /// <br/>It won't return the existing item that resides in
    /// the global list.
    /// </param>
    public delegate void CollectionUpdateItemDelegate<ItemType, NewItemType>(ItemType item, Func<NewItemType> createOrGetNewItem);
}
