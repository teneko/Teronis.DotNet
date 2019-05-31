using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Data
{
    public class KnownParentsContainer
    {
        public IHaveKnownParents HavingKnownParents { get; private set; }
        public ReadOnlyDictionary<object, WantParentsEventHandler> HandlerByCallerDictionary { get; private set; }

        private Dictionary<object, WantParentsEventHandler> handlerByCallerDictionary;

        public KnownParentsContainer(IHaveKnownParents havingKnownParents)
        {
            HavingKnownParents = havingKnownParents ?? throw new ArgumentNullException(nameof(havingKnownParents));
            handlerByCallerDictionary = new Dictionary<object, WantParentsEventHandler>();
            HandlerByCallerDictionary = new ReadOnlyDictionary<object, WantParentsEventHandler>(handlerByCallerDictionary);
        }

        public void AttachWantParentsHandler(object caller, WantParentsEventHandler handler)
        {
            if (handlerByCallerDictionary.ContainsKey(caller))
                throw new ArgumentException("The caller can only be once a parent");

            HavingKnownParents.AttachWantParentsHandler(handler);
            handlerByCallerDictionary.Add(caller, handler);
        }

        public void DetachWantParentsHandler(object caller)
        {
            if (!handlerByCallerDictionary.ContainsKey(caller))
                throw new ArgumentException("The caller has not been attached before");

            var handler = handlerByCallerDictionary[caller];
            HavingKnownParents.DetachWantParentsHandler(handler);
            handlerByCallerDictionary.Remove(caller);
        }
    }
}
