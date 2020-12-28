using Teronis.Collections.Synchronization.Example1.Models;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    /// <summary>
    /// It holds an observable collection of <see cref="DeviceHeaderSyntheticEntity"/>. Its purpose
    /// is to have a long running synced collection of <see cref="DeviceHeaderSyntheticEntity"/>.
    /// </summary>
    public class DeviceHeaderCollectionSynchronisation : SynchronizingCollection<DeviceHeaderViewModel, DeviceHeaderEntity>
    {
        public DeviceHeaderViewModel SelectedItem { get; set; }

#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private readonly AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<DeviceHeaderViewModel, DeviceHeaderEntity> collectionItemParentsBehaviour;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        public DeviceHeaderCollectionSynchronisation()
            : base(DeviceHeaderEntityEqualityComparer.Default)
        {
            collectionItemParentsBehaviour = new AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<DeviceHeaderViewModel, DeviceHeaderEntity>(this);
        }

        protected override DeviceHeaderViewModel CreateSubItem(DeviceHeaderEntity newItem) =>
            new DeviceHeaderViewModel(newItem);

        protected override void ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle modificationBundle)
        {
            foreach (var replaceItem in modificationBundle.OldSubItemsNewSuperItemsModification.GetReplaceItemsIterator()) {
                replaceItem.OldItem.Header = replaceItem.NewItem;
            }
        }
    }
}
