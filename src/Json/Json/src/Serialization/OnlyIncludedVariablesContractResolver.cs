using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Json.Serialization
{
    /// <summary>
    /// Special JsonConvert resolver that only resolves included properties.
    /// </summary>
    public class OnlyIncludedVariablesContractResolver : DefaultContractResolver
    {
        protected readonly Dictionary<Type, HashSet<string>> IncludedVariablesByTypeList;

        public OnlyIncludedVariablesContractResolver() => IncludedVariablesByTypeList = new Dictionary<Type, HashSet<string>>();

        /// <summary>
        /// Explicitly include the given property(s) for the given type
        /// </summary>
        /// <param name="declaringType">The type that declares the property</param>
        /// <param name="propertyName">One or more property to include. Leave it empty to include the entire type.</param>
        public void IncludeVariable(Type declaringType, params string[] propertyName)
        {
            // Start bucket if does not exist
            if (!IncludedVariablesByTypeList.ContainsKey(declaringType))
                IncludedVariablesByTypeList[declaringType] = new HashSet<string>();

            foreach (var prop in propertyName)
                IncludedVariablesByTypeList[declaringType].Add(prop);
        }

        /// <summary>
        /// Is the given property for the given type included?
        /// </summary>
        public bool IsVariableIncluded(Type type, string propertyName)
        {
            if (IncludedVariablesByTypeList.ContainsKey(type) && (IncludedVariablesByTypeList[type].Count == 0 || IncludedVariablesByTypeList[type].Contains(propertyName)))
                return true;

            return false;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            /// Need to check basetype as well for EF -- <see cref="IgnorableVariablesContractResolver"/>
            if (IsVariableIncluded(property.DeclaringType, property.PropertyName) || IsVariableIncluded(property.DeclaringType.BaseType, property.PropertyName))
                property.ShouldSerialize = instance => true;
            else
                property.ShouldSerialize = instance => false;

            return property;
        }
    }
}
