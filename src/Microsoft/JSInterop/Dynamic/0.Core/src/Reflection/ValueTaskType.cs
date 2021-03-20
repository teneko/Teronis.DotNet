using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    public partial class ValueTaskType
    {
        public Type? GenericParameterType { get; }
        public bool HasGenericParameterType => !(GenericParameterType is null);
        public Type Type { get; }

        public static ValueTaskType Parse(Type type)
        {
            if (type == typeof(ValueTask)) {
                return new ValueTaskType(type);
            }

            if (type.IsGenericType) {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                if (typeof(ValueTask<>) == genericTypeDefinition) {
                    var valueTaskGenericArgument = type.GetGenericArguments()[0];
                    return new ValueTaskType(type, valueTaskGenericArgument);
                }
            }

            throw ThrowHelper.CreateNotOfTypeValueTaskException(type);
        }

        internal ValueTaskType(Type valueTaskType)
        {
            Type = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
        }

        internal ValueTaskType(Type valueTaskType, Type genericArgumentType)
        {
            Type = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
            GenericParameterType = genericArgumentType ?? throw new ArgumentNullException(nameof(genericArgumentType));
        }
    }
}
