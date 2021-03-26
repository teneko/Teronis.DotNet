// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Teronis.Json.PropertyEntitifiers;
using Teronis.Reflection;
using Teronis.Utils;

namespace Teronis.Json.Converters
{
    /// <summary>
    /// This converter notifies instantiating classes about their property name. It supports 
    /// to deserialize a JSON object to <see cref="List{T}"/> or <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    /// <typeparam name="ValueType"></typeparam>
    public class JsonPropertyNameAnnouncingConverter<KeyType, ValueType> : JsonConverter
        where ValueType : IAnnounceJsonPropertyName<KeyType>
    {
        public override bool CanWrite => false;
        public IJsonPropertyEntitifiable<ValueType> JsonPropertyEntitifier { get; private set; }

        public JsonPropertyNameAnnouncingConverter(Type jsonPropertyReaderType)
        {
            if (!typeof(IJsonPropertyEntitifiable<ValueType>).IsAssignableFrom(jsonPropertyReaderType)) {
                throw new ArgumentException("Bad json property reader type.", nameof(jsonPropertyReaderType));
            }

            JsonPropertyEntitifier = Instantiator.Instantiate<IJsonPropertyEntitifiable<ValueType>>(jsonPropertyReaderType);
        }

        public JsonPropertyNameAnnouncingConverter() : this(typeof(DefaultJsonPropertyEntitifier<ValueType>)) { }

        private CollectionTypes getCollectionType(Type objectType)
        {
            if (typeof(IDictionary<KeyType, ValueType>).IsAssignableFrom(objectType)) {
                return CollectionTypes.Dictionary;
            } else if (typeof(IList<ValueType>).IsAssignableFrom(objectType)) {
                return CollectionTypes.List;
            } else {
                throw new NotImplementedException($"Type '{objectType}' not supported by {nameof(JsonPropertyNameAnnouncingConverter<KeyType, ValueType>)}.");
            }
        }

        /// <summary>
        /// Returns true if not an exception is thrown before.
        /// </summary>
        public override bool CanConvert(Type objectType)
            => TeronisUtils.ReturnInValue(true, () => getCollectionType(objectType));

        /// <returns>Return a <see cref="Dictionary{TKey, TValue}"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var collectionType = getCollectionType(objectType);

            if (reader.TokenType == JsonToken.StartObject) {
                Action<KeyType, ValueType> addCollectionItem;
                object collection;

                if (collectionType == CollectionTypes.Dictionary) {
                    var dictionary = new Dictionary<KeyType, ValueType>();
                    addCollectionItem = (key, value) => dictionary.Add(key, value);

                    if (objectType == typeof(ReadOnlyDictionary<KeyType, ValueType>)) {
                        collection = new ReadOnlyDictionary<KeyType, ValueType>(dictionary);
                    } else {
                        collection = dictionary;
                    }
                } else {
                    var list = new List<ValueType>();
                    addCollectionItem = (key, value) => list.Add(value);

                    if (objectType == typeof(ReadOnlyCollection<ValueType>)) {
                        collection = new ReadOnlyCollection<ValueType>(list);
                    } else {
                        collection = list;
                    }
                }

                var keyType = typeof(KeyType);
                var path = reader.Path;
                var lastPeriodIndex = path.Length;

                reader.Read();

                while (reader.Path != path && reader.Path.StartsWith(path)) {
                    if (reader.Path.LastIndexOf('.') == lastPeriodIndex) {
                        switch (reader.TokenType) {
                            case JsonToken.PropertyName:
                                var property = JProperty.Load(reader);
                                var key = (KeyType)Convert.ChangeType(property.Name, keyType);
                                var value = JsonPropertyEntitifier.EntitifyJsonProperty(property, serializer);
                                // TODO: look for key
                                value.AnnouncePropertyName(key);
                                addCollectionItem(key, value);
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                }

                return collection;
            } else {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();

        private enum CollectionTypes
        {
            Dictionary,
            List,
        }
    }
}
