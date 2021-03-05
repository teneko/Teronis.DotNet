using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.ObjectModel.Parenthood
{
    public class RegisteredRequestParentHandlerDictionary : IReadOnlyDictionary<object, ParentsRequestedEventHandler>
    {
        private readonly IHaveRegisteredParents havingRegisteredParents;
        private readonly Dictionary<object, ParentsRequestedEventHandler> handlerByCallerDictionary;

        public RegisteredRequestParentHandlerDictionary(IHaveRegisteredParents havingRegisteredParents)
        {
            this.havingRegisteredParents = havingRegisteredParents ?? throw new ArgumentNullException(nameof(havingRegisteredParents));
            handlerByCallerDictionary = new Dictionary<object, ParentsRequestedEventHandler>();
        }

        public void RegisterParent(object caller, ParentsRequestedEventHandler handler)
        {
            if (handlerByCallerDictionary.ContainsKey(caller)) {
                throw new ArgumentException("The caller can only be once a parent");
            }

            havingRegisteredParents.RegisterParent(handler);
            handlerByCallerDictionary.Add(caller, handler);
        }

        public void UnregisterParent(object caller)
        {
            if (!handlerByCallerDictionary.ContainsKey(caller)) {
                throw new ArgumentException("The caller has not been attached before");
            }

            var handler = handlerByCallerDictionary[caller];
            havingRegisteredParents.UnregisterParent(handler);
            handlerByCallerDictionary.Remove(caller);
        }

        #region IReadOnlyDictionary<object, ParentsRequestedEventHandler>

        public IEnumerable<object> Keys =>
           ((IReadOnlyDictionary<object, ParentsRequestedEventHandler>)handlerByCallerDictionary).Keys;

        public IEnumerable<ParentsRequestedEventHandler> Values =>
            ((IReadOnlyDictionary<object, ParentsRequestedEventHandler>)handlerByCallerDictionary).Values;

        public int Count =>
            ((IReadOnlyCollection<KeyValuePair<object, ParentsRequestedEventHandler>>)handlerByCallerDictionary).Count;

        public ParentsRequestedEventHandler this[object key] =>
            ((IReadOnlyDictionary<object, ParentsRequestedEventHandler>)handlerByCallerDictionary)[key];

        public bool ContainsKey(object key) =>
            ((IReadOnlyDictionary<object, ParentsRequestedEventHandler>)handlerByCallerDictionary).ContainsKey(key);

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out ParentsRequestedEventHandler value) =>
            ((IReadOnlyDictionary<object, ParentsRequestedEventHandler>)handlerByCallerDictionary).TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<object, ParentsRequestedEventHandler>> GetEnumerator() =>
            ((IEnumerable<KeyValuePair<object, ParentsRequestedEventHandler>>)handlerByCallerDictionary).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)handlerByCallerDictionary).GetEnumerator();

        #endregion
    }
}
