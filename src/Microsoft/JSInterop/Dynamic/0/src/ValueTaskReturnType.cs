using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public partial class ValueTaskReturnType
    {
        public Type? GenericParameterType { get; }
        public bool HasGenericParameterType => !(GenericParameterType is null);
        public Type Type { get; }

        public static ValueTaskReturnType Parse(Type type) {
            if (type == typeof(ValueTask)) {
                return new ValueTaskReturnType(type);
            }

            if (type.IsGenericType) {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                if (typeof(ValueTask<>) == genericTypeDefinition) {
                    var valueTaskGenericArgument = type.GetGenericArguments()[0];
                    return new ValueTaskReturnType(type, valueTaskGenericArgument);
                }
            }

            throw ThrowHelper.CreateNotOfTypeValueTaskException(type);
        }

        internal ValueTaskReturnType(Type valueTaskType) {
            Type = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
        }

        internal ValueTaskReturnType(Type valueTaskType, Type genericArgumentType) {
            Type = valueTaskType ?? throw new ArgumentNullException(nameof(valueTaskType));
            GenericParameterType = genericArgumentType ?? throw new ArgumentNullException(nameof(genericArgumentType));
        }
    }
}
