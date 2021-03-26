// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Teronis.Text.Json.Serialization;

namespace Teronis.Json.Serialization
{
    /// <summary>
    /// Special JsonConvert resolver that only resolves included properties.
    /// </summary>
    public class IncludedVariablesContractResolver : DefaultContractResolver, IVariablesClusionHelper
    {
        protected Dictionary<Type, HashSet<string>> IncludedVariablesByTypeList =>
            variablesInclusionHelper.VariablesByTypeList;

        private readonly VariablesClusionHelper variablesInclusionHelper;

        public IncludedVariablesContractResolver() =>
            variablesInclusionHelper = new VariablesClusionHelper();

        public IncludedVariablesContractResolver(IEnumerable<KeyValuePair<Type, string>> includedVariables) =>
            variablesInclusionHelper = new VariablesClusionHelper(includedVariables);

        /// <summary>
        /// Explicitly include the given property(s) for the given type
        /// </summary>
        /// <param name="declaringType">The type that declares the property</param>
        /// <param name="propertyName">One or more property to include. Leave it empty to include the entire type.</param>
        public void IncludeVariable(Type declaringType, params string[] propertyName) =>
            variablesInclusionHelper.ConsiderVariable(declaringType, propertyName);

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (variablesInclusionHelper.IsVariableConsidered(property.DeclaringType, property.PropertyName)) {
                property.ShouldSerialize = instance => true;
            } else {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }

        void IVariablesClusionHelper.ConsiderVariable(Type declaringType, params string[] propertyName)
            => IncludeVariable(declaringType, propertyName);
    }
}
