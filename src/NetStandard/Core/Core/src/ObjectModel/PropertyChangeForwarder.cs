// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.ComponentModel
{
    public abstract class PropertyChangeForwarder<EventArgumentType> : EventInvocationForwarder<object?, EventArgumentType>
    {
        protected Dictionary<string, string> CalleePropertyNameByCallerPropertyNameDictionary;

        public PropertyChangeForwarder(object? alternativeEventSender)
            : base(alternativeEventSender) =>
            CalleePropertyNameByCallerPropertyNameDictionary = new Dictionary<string, string>();

        public PropertyChangeForwarder()
            : base() =>
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
