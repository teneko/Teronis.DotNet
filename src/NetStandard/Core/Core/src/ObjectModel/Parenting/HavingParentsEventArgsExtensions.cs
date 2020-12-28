using System.Collections.Generic;

namespace Teronis.ObjectModel.Parenting
{
    public static class HavingParentsEventArgsExtensions
    {
        /// <summary>
        /// Does only attach the parent to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AddParent(this HavingParentsEventArgs args, object parent)
            => args.Container.AddParent(parent);

        /// <summary>
        /// Does only attach the parents to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AddParents(this HavingParentsEventArgs args, IEnumerable<object> parents)
        {
            foreach (var parent in parents) {
                AddParent(args, parent);
            }
        }

        /// <summary>
        /// Does only attach the parents to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AddParents(this HavingParentsEventArgs args, params object[] parents)
            => AddParents(args, (IEnumerable<object>)parents);

        /// <summary>
        /// Does attach the parent to the parent container and does look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AddParentAndItsParents(this HavingParentsEventArgs args, object parent)
        {
            AddParent(args, parent);

            if (parent is IHaveParents havingParents) {
                var parents = havingParents.CreateParentsCollector().CollectParents(args.Container.WantedType);
                args.Container.AddParents(parents);
            }
        }

        /// <summary>
        /// Does attach the parents to the parent container and does look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AddParentsAndTheirParents(this HavingParentsEventArgs args, IEnumerable<object> parents)
        {
            foreach (var parent in parents) {
                AddParentAndItsParents(args, parent);
            }
        }
    }
}
