// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teronis.Json.PropertyEntitifiers
{
    public interface IJsonPropertyEntitifiable<EntityType>
    {
        EntityType EntitifyJsonProperty(JProperty property, JsonSerializer serializer);
    }
}
