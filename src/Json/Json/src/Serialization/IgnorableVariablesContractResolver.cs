using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Json.Serialization
{
    /// <summary>
    /// Special JsonConvert resolver that allows you to ignore properties.  See https://stackoverflow.com/a/13588192/1037948
    /// </summary>
    public class IgnorableVariablesContractResolver : DefaultContractResolver
    {
        protected readonly Dictionary<Type, HashSet<string>> IgnorableVariablesByTypeList;

        public IgnorableVariablesContractResolver() => IgnorableVariablesByTypeList = new Dictionary<Type, HashSet<string>>();

        /// <summary>
        /// Explicitly ignore the given property(s) for the given type
        /// </summary>
        /// <param name="propertyName">one or more properties to ignore.  Leave empty to ignore the type entirely.</param>
        public void IgnoreVariable(Type type, params string[] propertyName)
        {
            // Start bucket if does not exist
            if (!IgnorableVariablesByTypeList.ContainsKey(type))
                IgnorableVariablesByTypeList[type] = new HashSet<string>();

            foreach (var prop in propertyName)
                IgnorableVariablesByTypeList[type].Add(prop);
        }

        /// <summary>
        /// Is the given property for the given type ignored?
        /// </summary>
        public bool IsVariableIgnored(Type type, string propertyName)
        {
            if (!IgnorableVariablesByTypeList.ContainsKey(type))
                return false;

            // if no properties provided, ignore the type entirely
            if (IgnorableVariablesByTypeList[type].Count == 0)
                return true;

            return IgnorableVariablesByTypeList[type].Contains(propertyName);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Need to check basetype as well for EF -- @per comment by user576838
            if (IsVariableIgnored(property.DeclaringType, property.PropertyName) || IsVariableIgnored(property.DeclaringType.BaseType, property.PropertyName))
                property.ShouldSerialize = instance => false;

            return property;
        }
    }
}
