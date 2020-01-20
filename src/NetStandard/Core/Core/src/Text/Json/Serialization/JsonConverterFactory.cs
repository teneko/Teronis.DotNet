using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Teronis.Text.Json.Serialization
{
    public class JsonConverterFactory
    {
        private static JsonConverter wrap(JsonConverter converter, out IVariablesClusionHelper variablesHelper)
        {
            variablesHelper = (IVariablesClusionHelper)converter;
            return converter;
        }

        private static JsonConverter createOnlyIncludedVariablesJsonConverter(Type type, params object[] args)
        {
            var genericTemplateType = typeof(OnlyIncludedVariablesJsonConverter<>);
            var genericType = genericTemplateType.MakeGenericType(type);
            return (JsonConverter)Activator.CreateInstance(genericType, args);
        }

        public static JsonConverter CreateOnlyIncludedVariablesJsonConverter(Type type, out IVariablesClusionHelper variablesInclusionHelper) =>
            wrap(createOnlyIncludedVariablesJsonConverter(type), out variablesInclusionHelper);

        public static JsonConverter CreateOnlyIncludedVariablesJsonConverter(Type type, IEnumerable<KeyValuePair<Type, string>> includedVariables, out IVariablesClusionHelper variablesInclusionHelper) =>
            wrap(createOnlyIncludedVariablesJsonConverter(type, includedVariables), out variablesInclusionHelper);
    }
}
