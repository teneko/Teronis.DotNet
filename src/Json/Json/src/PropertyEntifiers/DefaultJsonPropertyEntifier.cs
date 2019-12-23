using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teronis.Json.PropertyEntifiers
{
    public class DefaultJsonPropertyEntifier<ValueType> : IEntitifyJsonProperty<ValueType>
    {
        /// <param name="property"></param>
        /// <returns>Expect a value.</returns>
        public ValueType EntitifyJsonProperty(JProperty property, JsonSerializer serializer)
        {
            return property.Value.ToObject<ValueType>(serializer);
        }
    }
}
