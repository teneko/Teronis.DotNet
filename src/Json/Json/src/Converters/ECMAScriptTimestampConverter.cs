using Newtonsoft.Json;
using System;
using Teronis.Tools.NetStandard;
using Teronis.Extensions.NetStandard;

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
            if (objectType.IsNullable())
                objectType = Nullable.GetUnderlyingType(objectType);

            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            var isValueNull = value == null;
            var isObjectTypeNullable = objectType.IsNullable();

            if (isValueNull && isObjectTypeNullable)
                return null;
            else {
                try {
                    if (!isValueNull && double.TryParse(reader.Value.ToString(), out double timeOffset))
                        return DateTimeTools.ECMAScriptTimestampToDateTime(timeOffset);
                    else
                        throw new ArgumentException("Value is not a ECMAScript valid time value");
                } catch (Exception error) {
                    if (IsExceptionUnwanted) {
                        if (isObjectTypeNullable)
                            return null;
                        else
                            return new DateTime();
                    } else
                        throw error;
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            object ttbWrittenValue;

            if (value == null)
                ttbWrittenValue = null;
            else {
                var dateTime = (DateTime)value;
                ttbWrittenValue = DateTimeTools.DateTimeToECMAScriptTimestamp(dateTime);
            }

            writer.WriteValue(ttbWrittenValue);
        }
    }
}
