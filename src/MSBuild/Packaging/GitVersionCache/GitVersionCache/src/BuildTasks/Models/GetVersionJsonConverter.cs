using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GitVersion.MSBuildTask.Tasks;

namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public class GetVersionJsonConverter : JsonConverter<GetVersion>
    {
        public override GetVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, GetVersion value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
