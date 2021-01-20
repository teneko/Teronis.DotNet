using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms.Algorithms;
using Teronis.Extensions;

namespace Teronis.NetStandard.Collections.Algorithms.Test
{
    public class SynchronizableCollection<T> : List<T>
        where T : notnull
    {
        public SynchronizableCollection(IEnumerable<T> collection)
            : base(collection) { }

        public void SynchronizeCollection(IEnumerable<T> enumerable, bool preventYielding)
        {
            var modifications = EqualityTrailingCollectionModifications.YieldCollectionModifications(this, enumerable);

            if (preventYielding) {
                modifications = modifications.ToList();  
            }

            foreach (var modification in modifications) {
                switch (modification.Action) {
                    case NotifyCollectionChangedAction.Add: {
                            var index = modification.NewIndex;

                            foreach (var newItem in modification.NewItems!) {
                                Insert(index++, newItem);
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove: {
                            var index = modification.OldIndex + modification.OldItems!.Count - 1;

                            foreach (var newItem in modification.OldItems) {
                                RemoveAt(index--);
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Replace: {
                            var index = modification.NewIndex;

                            foreach (var newItem in modification.NewItems!) {
                                this[index] = newItem;
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                        this.Move(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Clear();
                        break;
                }
            }
        }

        public void SynchronizeCollection(IEnumerable<T> enumerable) =>
            SynchronizeCollection(enumerable, preventYielding: false);
    }
}
