using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teronis.Json.PropertyEntitifiers
{
    public interface IJsonPropertyEntitifiable<EntityType>
    {
        EntityType EntitifyJsonProperty(JProperty property, JsonSerializer serializer);
    }
}
