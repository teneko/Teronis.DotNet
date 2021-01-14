using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Changes;
using Teronis.Extensions;

namespace Teronis.NetStandard.Collections.Changes.Test
{
    public class SynchronizableCollection<T> : List<T>
    {
        public SynchronizableCollection(IEnumerable<T> collection)
            : base(collection) { }

        public void SynchronizeCollection(IEnumerable<T> enumerable)
        {
            var modifications = CollectionModifications.YieldCollectionModifications(this, enumerable);

            foreach (var modification in modifications) {
                switch (modification.Action) {
                    case NotifyCollectionChangedAction.Add: {
                            var index = modification.NewIndex;

                            foreach (var newItem in modification.NewItems) {
                                Insert(index++, newItem);
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove: {
                            var index = modification.OldIndex + modification.OldItems.Count - 1;

                            foreach (var newItem in modification.OldItems) {
                                RemoveAt(index--);
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Replace: {
                            var index = modification.NewIndex;

                            foreach (var newItem in modification.NewItems) {
                                this[index] = newItem;
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                        this.Move(modification.OldIndex, modification.NewIndex, modification.OldItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Clear();
                        break;
                }
            }
        }
    }
}
