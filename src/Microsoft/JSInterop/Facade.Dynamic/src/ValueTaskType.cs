using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    internal partial class ValueTaskType
    {
        public Type? GenericParameterType { get; }
        public bool HasGenericParameterType => !(GenericParameterType is null);
        public Type Type { get; }

        public static ValueTaskType Parse(Type type) {
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

        public ValueTaskType(Type type) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public ValueTaskType(Type type, Type genericType) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            GenericParameterType = genericType ?? throw new ArgumentNullException(nameof(genericType));
        }
    }
}
