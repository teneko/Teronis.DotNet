using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Teronis.Text.Json.Serialization;

namespace Teronis.Json.Serialization
{
    /// <summary>
    /// Special JsonConvert resolver that allows you to ignore properties.  See https://stackoverflow.com/a/13588192/1037948
    /// </summary>
    public class IgnorableVariablesContractResolver : DefaultContractResolver, IVariablesClusionHelper
    {
        // #####

        protected Dictionary<Type, HashSet<string>> IgnorableVariablesByTypeList =>
            variablesExclusionHelper.VariablesByTypeList;

        private readonly VariablesClusionHelper variablesExclusionHelper;

        public IgnorableVariablesContractResolver() =>
            variablesExclusionHelper = new VariablesClusionHelper();

        public IgnorableVariablesContractResolver(IEnumerable<KeyValuePair<Type, string>> includedVariables) =>
            variablesExclusionHelper = new VariablesClusionHelper(includedVariables);

        /// <summary>
        /// Explicitly ignore the given property(s) for the given type
        /// </summary>
        /// <param name="propertyName">one or more properties to ignore.  Leave empty to ignore the type entirely.</param>
        public void IgnoreVariable(Type declaringType, params string[] propertyName) =>
            variablesExclusionHelper.ConsiderVariable(declaringType, propertyName);

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (variablesExclusionHelper.IsVariableConsidered(property.DeclaringType, property.PropertyName)) {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }

        void IVariablesClusionHelper.ConsiderVariable(Type declaringType, params string[] propertyName)
            => IgnoreVariable(declaringType, propertyName);
    }
}
