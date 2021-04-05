// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Newtonsoft.Json;
using Teronis.Extensions;
using Teronis.Reflection;

namespace Teronis.Json.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerSettings GetSettings(this JsonSerializer serializer)
        {
            var jsonSerializerSettingsVariableInfoSettings = new VariableMemberDescriptor() {
                IncludeIfWritable = true,
            };

            var serializerSettingsVariableInfoByNameList = typeof(JsonSerializerSettings)
                .GetVariableMembers(descriptor: jsonSerializerSettingsVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var jsonSerializerVariableInfoSettings = new VariableMemberDescriptor() {
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
