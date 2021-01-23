using System;
using System.Collections.Generic;

namespace Teronis.ObjectModel
{
    public abstract class PropertyChangeForwarder<ForwardingEventContainerType, EventArgumentType> : EventInvocationForwarder<ForwardingEventContainerType, EventArgumentType>
    {
        protected Dictionary<string, string> CalleePropertyNameByCallerPropertyNameDictionary;

        public PropertyChangeForwarder(ForwardingEventContainerType forwardingEventContainer, object? alternativeEventSender = null)
            : base(forwardingEventContainer, alternativeEventSender: alternativeEventSender) =>
            CalleePropertyNameByCallerPropertyNameDictionary = new Dictionary<string, string>();

        public void AddPropertyChangeForwarding(string callerPropertyName, string calleePropertyName)
        {
            if (CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(callerPropertyName)) {
                throw new ArgumentException("Caller property name is already registered.");
            }

            CalleePropertyNameByCallerPropertyNameDictionary.Add(callerPropertyName, calleePropertyName);
        }

        public void AddPropertyChangeForwarding(string callerAndCalleePropertyName) =>
            AddPropertyChangeForwarding(callerAndCalleePropertyName, callerAndCalleePropertyName);

        public void RemovePropertyChangeForwarding(string callerPropertyName)
        {
            if (!CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(callerPropertyName)) {
                throw new ArgumentException("Caller property name is not registered.");
            }

            CalleePropertyNameByCallerPropertyNameDictionary.Remove(callerPropertyName);
        }
    }
}
