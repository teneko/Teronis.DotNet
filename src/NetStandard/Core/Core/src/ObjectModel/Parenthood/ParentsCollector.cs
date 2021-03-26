// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Teronis.ObjectModel.Parenthood
{
    public class ParentsCollector
    {
        public object Sender { get; private set; }

        private readonly ParentsRequestedEventHandler? eventHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventHandler">The event handler who requests parents for <paramref name="sender"/>.</param>
        public ParentsCollector(object sender, ParentsRequestedEventHandler? eventHandler)
        {
            Sender = sender;
            this.eventHandler = eventHandler;
        }

        public ParentsContainer.ParentCollection CollectParents(Type? requestedParentType)
        {
            var args = new HavingParentsEventArgs(Sender, requestedParentType);
            eventHandler?.Invoke(args.OriginalSource, args);
            return args.Container.Parents;
        }

        public RequestedParentType SingleParent<RequestedParentType>()
            => (RequestedParentType)CollectParents(typeof(RequestedParentType)).Single();

        public RequestedParentType FirstParent<RequestedParentType>()
            => (RequestedParentType)CollectParents(typeof(RequestedParentType)).First();
    }
}
