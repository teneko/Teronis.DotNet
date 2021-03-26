// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;
using Teronis.Json.Extensions;
using Teronis.Utils;

namespace Teronis.Json.Converters
{
    public class ExceptionalDefaultValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try {
                var serializerSettings = serializer.GetSettings();
                return reader.Value.ToString().DeserializeJson(objectType, serializerSettings);
            } catch {
                return TypeUtils.GetDefaultOfValueOrReferenceType(objectType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
