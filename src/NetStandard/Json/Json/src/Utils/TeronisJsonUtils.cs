using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Teronis.Extensions;
using Teronis.Json.Extensions;
using Teronis.Json.Serialization;

namespace Teronis.Json.Utils
{
    public class TeronisJsonUtils
    {
        /// <summary>
        /// Deserialize only a part of an instance with the type of <paramref name="containerType"/> by one or more "variable name and its content"-pairs.
        /// </summary>
        /// <param name="containerType">The type of the container which contains all "variable name and its content"-pairs</param>
        /// <param name="settings">The settings when serializing and deserializing the dummy dictionary. Passing null is handled by Json.NET.</param>
        /// <param name="variableNameAndContentPairs">The variable members of the container</param>
        public static object GetDeserializePropertiesContainer(Type containerType, JsonSerializerSettings settings, params KeyValuePair<string, object>[] variableNameAndContentPairs)
        {
            var dummyDictionary = new Dictionary<string, object>();
            var dummyCollection = (ICollection<KeyValuePair<string, object>>)dummyDictionary;
            var contractResolver = new IncludedVariablesContractResolver();

            foreach (var variableNameAndContentPair in variableNameAndContentPairs) {
                // Here we attach the hole key value pair to the dictionary
                dummyCollection.Add(variableNameAndContentPair);
                contractResolver.IncludeVariable(containerType, variableNameAndContentPair.Key);
            }

            var deserializedDuckContainer = dummyDictionary.SerializeObject(settings);

            JsonSerializerSettings copiedSettings;

            // Settings can be null, so do not copy it when null
            if (settings != null) {
                copiedSettings = settings.ShallowCopy();
                copiedSettings.ContractResolver = contractResolver;
            } else {
                copiedSettings = null;
            }

            return deserializedDuckContainer.DeserializeJson(containerType, copiedSettings);
        }

        /// <summary>
        /// Deserialize only a part of an instance with the type of <paramref name="containerType"/> by one or more "variable name and its content"-pairs.
        /// </summary>
        /// <param name="containerType">The type of the container which contains all "variable name and its content"-pairs</param>
        /// <param name="variableNameAndContentPairs">The variable members of the container</param>
        public static object GetDeserializePropertiesContainer(Type containerType, params KeyValuePair<string, object>[] variableNameAndContentPairs)
            => GetDeserializePropertiesContainer(containerType, null, variableNameAndContentPairs);

        /// <summary>
        /// Deserialize only a part of an instance with the type of <paramref name="declaringType"/> by one or more "variable name and its content"-pairs.
        /// </summary>
        /// <param name="declaringType">The type of the container which contains all "variable name and its content"-pairs</param>
        /// <param name="settings">The settings when serializing and deserializing the dummy dictionary. Passing null is handled by Json.NET.</param>
        /// <param name="variableNameAndContentPairs">The variable members of the container</param>
        public static object GetDeserializedPropertyContainer(Type containerType, string variableName, object content, JsonSerializerSettings settings)
            => GetDeserializePropertiesContainer(containerType, settings, new KeyValuePair<string, object>(variableName, content));

        /// <summary>
        /// Deserialize only a part of an instance with the type of <paramref name="declaringType"/> by one or more "variable name and its content"-pairs.
        /// </summary>
        /// <param name="declaringType">The type of the container which contains all "variable name and its content"-pairs</param>
        /// <param name="settings">The settings when serializing and deserializing the dummy dictionary</param>
        /// <param name="variableNameAndContentPairs">The variable members of the container</param>
        public static object GetDeserializedPropertyContainer(Type containerType, string variableName, object content)
            => GetDeserializedPropertyContainer(containerType, variableName, content, null);
    }
}
