using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Teronis.Reflection;
using Teronis.Extensions.NetStandard;

namespace Teronis.Json.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerSettings GetSettings(this JsonSerializer serializer)
        {
            var jsonSerializerSettingsVariableInfoSettings = new VariableInfoSettings() {
                IncludeIfWritable = true,
            };

            var serializerSettingsVariableInfoByNameList = typeof(JsonSerializerSettings)
                .GetVariableMembers(jsonSerializerSettingsVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var jsonSerializerVariableInfoSettings = new VariableInfoSettings() {
                IncludeIfReadable = true,
            };

            var serializerVariableInfoByNameList = typeof(JsonSerializer)
                .GetVariableMembers(jsonSerializerVariableInfoSettings)
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
