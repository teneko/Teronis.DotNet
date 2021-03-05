using System;
using Newtonsoft.Json;
using Teronis.Utils;

namespace Teronis.Json.Converters
{
    public class ECMAScriptTimestampConverter : JsonConverter
    {
        public bool IsExceptionUnwanted { get; private set; }

        public ECMAScriptTimestampConverter(bool isExceptionUnwanted)
            => IsExceptionUnwanted = isExceptionUnwanted;

        public ECMAScriptTimestampConverter() : this(false) { }

        public override bool CanConvert(Type objectType)
        {
            if (TypeUtils.IsNullable(objectType)) {
                objectType = Nullable.GetUnderlyingType(objectType);
            }

            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            var isValueNull = value == null;
            var isObjectTypeNullable = TypeUtils.IsNullable(objectType);

            if (isValueNull && isObjectTypeNullable) {
                return null;
            } else {
                try {
                    if (!isValueNull && double.TryParse(reader.Value.ToString(), out double timeOffset)) {
                        return DateTimeUtils.ECMAScriptTimestampToDateTime(timeOffset);
                    } else {
                        throw new ArgumentException("Value is not a ECMAScript valid time value");
                    }
                } catch (Exception) {
                    if (IsExceptionUnwanted) {
                        if (isObjectTypeNullable) {
                            return null;
                        } else {
                            return new DateTime();
                        }
                    } else {
                        throw;
                    }
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            object ttbWrittenValue;

            if (value == null) {
                ttbWrittenValue = null;
            } else {
                var dateTime = (DateTime)value;
                ttbWrittenValue = DateTimeUtils.DateTimeToECMAScriptTimestamp(dateTime);
            }

            writer.WriteValue(ttbWrittenValue);
        }
    }
}
