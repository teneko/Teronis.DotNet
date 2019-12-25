using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Teronis.Extensions;
using Teronis.Json.PropertyEntifiers;
using Teronis.Tools;

namespace Teronis.Json.Converters
{
    /// <summary>
    /// Use it, when you want to notify a deserializing class about the property key to whom it belongs.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DeserializedKeyNotifierConverter<TKey, TValue> : JsonConverter
        where TValue : INotifyDeserializedJsonKey<TKey>
    {
        public override bool CanWrite => false;
        public IEntitifyJsonProperty<TValue> JsonPropertyEntifier { get; private set; }

        public DeserializedKeyNotifierConverter(Type jsonPropertyReaderType)
        {
            if (!typeof(IEntitifyJsonProperty<TValue>).IsAssignableFrom(jsonPropertyReaderType))
                throw new ArgumentException("Bad json property reader type.", nameof(jsonPropertyReaderType));

            JsonPropertyEntifier = (IEntitifyJsonProperty<TValue>)jsonPropertyReaderType.InstantiateUninitializedObject();
        }

        public DeserializedKeyNotifierConverter() : this(typeof(DefaultJsonPropertyEntifier<TValue>)) { }

        private ECollectionType getCollectionType(Type objectType)
        {
            if (typeof(IDictionary<TKey, TValue>).IsAssignableFrom(objectType))
                return ECollectionType.Dictionary;
            else if (typeof(IList<TValue>).IsAssignableFrom(objectType))
                return ECollectionType.List;
            else
                throw new NotImplementedException($"Type '{objectType}' not supported by {nameof(DeserializedKeyNotifierConverter<TKey, TValue>)}.");
        }

        /// <summary>
        /// Returns true if not an exception is thrown before.
        /// </summary>
        public override bool CanConvert(Type objectType)
            => TeronisTools.ReturnInValue(true, () => getCollectionType(objectType));

        /// <returns>Return a <see cref="Dictionary{TKey, TValue}"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var collectionType = getCollectionType(objectType);

            if (reader.TokenType == JsonToken.StartObject) {
                Action<TKey, TValue> addCollectionItem;
                object collection;

                if (collectionType == ECollectionType.Dictionary) {
                    var dictionary = new Dictionary<TKey, TValue>();
                    addCollectionItem = (key, value) => dictionary.Add(key, value);

                    if (objectType == typeof(ReadOnlyDictionary<TKey, TValue>))
                        collection = new ReadOnlyDictionary<TKey, TValue>(dictionary);
                    else
                        collection = dictionary;
                } else {
                    var list = new List<TValue>();
                    addCollectionItem = (key, value) => list.Add(value);

                    if (objectType == typeof(ReadOnlyCollection<TValue>))
                        collection = new ReadOnlyCollection<TValue>(list);
                    else
                        collection = list;
                }

                var keyType = typeof(TKey);
                var path = reader.Path;
                var lastPeriodIndex = path.Length;

                reader.Read();

                while (reader.Path != path && reader.Path.StartsWith(path)) {
                    if (reader.Path.LastIndexOf('.') == lastPeriodIndex) {
                        switch (reader.TokenType) {
                            case JsonToken.PropertyName:
                                var property = JProperty.Load(reader);
                                var key = Convert.ChangeType(property.Name, keyType).CastTo<TKey>();
                                var value = JsonPropertyEntifier.EntitifyJsonProperty(property, serializer);
                                // TODO: look for key
                                value.NotifyDeserializedJsonKey(key);
                                addCollectionItem(key, value);
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                }

                return collection;
            } else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();

        private enum ECollectionType
        {
            Dictionary,
            List,
        }
    }
}
