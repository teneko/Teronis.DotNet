using System;
using Newtonsoft.Json;

namespace Teronis.Json.Converters
{
    public class TruthyValueConverter : JsonConverter
    {
        public object ComparisonValue { get; private set; }

        public TruthyValueConverter(object comparisonValue)
            => ComparisonValue = comparisonValue;

        public override bool CanConvert(Type objectType)
            => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => reader.Value == ComparisonValue ? true : false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
    }
}
