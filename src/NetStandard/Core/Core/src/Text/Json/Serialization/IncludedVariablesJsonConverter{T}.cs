// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Teronis.Extensions;
using Teronis.Reflection;

namespace Teronis.Text.Json.Serialization
{
    public class IncludedVariablesJsonConverter<T> : JsonConverter<T>, IVariablesClusionHelper
    {
        protected Dictionary<Type, HashSet<string>> IncludedVariablesByTypeList =>
           variablesInclusionHelper.VariablesByTypeList;

        private readonly VariablesClusionHelper variablesInclusionHelper;

        public IncludedVariablesJsonConverter() =>
            variablesInclusionHelper = new VariablesClusionHelper();

        public IncludedVariablesJsonConverter(IEnumerable<KeyValuePair<Type, string>> includedVariables) =>
            variablesInclusionHelper = new VariablesClusionHelper(includedVariables);

        /// <summary>
        /// Explicitly include the given property(s) for the given type
        /// </summary>
        /// <param name="declaringType">The type that declares the property</param>
        /// <param name="propertyName">One or more property to include. Leave it empty to include the entire type.</param>
        public void IncludeVariable(Type declaringType, params string[] propertyName) =>
            variablesInclusionHelper.ConsiderVariable(declaringType, propertyName);

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            using var document = JsonDocument.Parse(JsonSerializer.Serialize(value));
            var valueType = typeof(T);

            var variableSettings = new VariableMemberDescriptor() {
                Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            };

            foreach (var property in document.RootElement.EnumerateObject()) {
                var memberInfo = valueType.GetVariableMember(property.Name, variableSettings);

                if (memberInfo?.DeclaringType == null || !variablesInclusionHelper.IsVariableConsidered(memberInfo.DeclaringType, property.Name)) {
                    continue;
                }

                property.WriteTo(writer);
            }

            writer.WriteEndObject();
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            (T)JsonSerializer.Deserialize(reader.GetString()!, typeToConvert, options)!;

        void IVariablesClusionHelper.ConsiderVariable(Type declaringType, params string[] propertyName)
            => IncludeVariable(declaringType, propertyName);
    }
}
