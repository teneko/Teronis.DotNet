using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Tools
{
    public static class TypeTools
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static object? InstantiateUninitializedObject(Type type)
        {
            var instantiatorType = typeof(Instantiator<>);
            var genericInstantiatorType = instantiatorType.MakeGenericType(type);
            var instantiateMethodName = nameof(Instantiator<object>.Instantiate);
            var instanteMethodBindingFlags = BindingFlags.Public | BindingFlags.Static;
            var instantiateMethod = genericInstantiatorType.GetMethod(instantiateMethodName, instanteMethodBindingFlags);
            return instantiateMethod?.Invoke(null, null);
        }

        public static object? GetDefault(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type.IsValueType) {
                return InstantiateUninitializedObject(type);
            }

            return null;
        }

        public static IEnumerable<Type> GetBaseTypes(Type type, Type? interruptingBaseType = null)
        {
            if (type == null) {
                yield break;
            }

            var nextType = type;
            var objectType = typeof(object);

            for (; ; ) {
                yield return nextType;
                nextType = nextType.BaseType;

                if (nextType == null || nextType == interruptingBaseType || nextType == objectType) {
                    break;
                }
            }
        }
    }
}
