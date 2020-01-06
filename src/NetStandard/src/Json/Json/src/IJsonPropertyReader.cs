using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teronis.Json
{
    public interface IEntitifyJsonProperty<EntityType>
    {
        EntityType EntitifyJsonProperty(JProperty property, JsonSerializer serializer);
    }
}
