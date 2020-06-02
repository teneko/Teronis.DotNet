using Newtonsoft.Json;
using System.Linq;
using Teronis.Reflection;
using Teronis.Extensions;

namespace Teronis.Json.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerSettings GetSettings(this JsonSerializer serializer)
        {
            var jsonSerializerSettingsVariableInfoSettings = new VariableInfoDescriptor() {
                IncludeIfWritable = true,
            };

            var serializerSettingsVariableInfoByNameList = typeof(JsonSerializerSettings)
                .GetVariableMembers(descriptor: jsonSerializerSettingsVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var jsonSerializerVariableInfoSettings = new VariableInfoDescriptor() {
                IncludeIfReadable = true,
            };

            var serializerVariableInfoByNameList = typeof(JsonSerializer)
                .GetVariableMembers(descriptor: jsonSerializerVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var serializerSettings = new JsonSerializerSettings();

            foreach (var nameAndSerializerSettingsVariableInfoPair in serializerSettingsVariableInfoByNameList) {
                var serializerSettingsVariableInfoKey = nameAndSerializerSettingsVariableInfoPair.Key;

                if (serializerVariableInfoByNameList.ContainsKey(serializerSettingsVariableInfoKey)) {
                    var serializerSettingsVariableInfo = nameAndSerializerSettingsVariableInfoPair.Value;
                    var serializerVariableInfo = serializerVariableInfoByNameList[serializerSettingsVariableInfoKey];
                    var serializerVariableValue = serializerVariableInfo.GetValue(serializer);

                    serializerSettingsVariableInfo.SetValue(serializerSettings, serializerVariableValue);
                }
            }

            return serializerSettings;
        }
    }
}
