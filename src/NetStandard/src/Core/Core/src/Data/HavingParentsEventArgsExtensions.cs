using System.Collections.Generic;
using Teronis.Data;

namespace Teronis.Extensions
{
    public static class HavingParentsEventArgsExtensions
    {
        /// <summary>
        /// Does only attach the parent to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AttachParent(this HavingParentsEventArgs args, object parent)
            => args.Container.AddParent(parent);

        /// <summary>
        /// Does only attach the parents to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AttachParents(this HavingParentsEventArgs args, IEnumerable<object> parents)
        {
            foreach (var parent in parents)
                AttachParent(args, parent);
        }

        /// <summary>
        /// Does only attach the parents to the parent container and does not look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AttachParents(this HavingParentsEventArgs args, params object[] parents)
            => AttachParents(args, (IEnumerable<object>) parents);

        /// <summary>
        /// Does attach the parent to the parent container and does look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AttachParentParents(this HavingParentsEventArgs args, object parent)
        {
            AttachParent(args, parent);

            if (parent is IHaveParents havingParents) {
                var parents = havingParents.GetParentsPicker().GetParents(args.Container.WantedType);
                args.Container.AddParents(parents);
            }
        }

        /// <summary>
        /// Does attach the parents to the parent container and does look for <see cref="IHaveParents"/> implementation.
        /// </summary>
        public static void AttachParentsParents(this HavingParentsEventArgs args, IEnumerable<object> parents)
        {
            foreach (var parent in parents)
                AttachParentParents(args, parent);
        }

        public static ParentsContainer.ParentCollection GetParents(this HavingParentsEventArgs args, WantParentsEventHandler eventHandler)
        {
            eventHandler?.Invoke(args.OriginalSource, args);
            return args.Container.Parents;
        }
    }
}
