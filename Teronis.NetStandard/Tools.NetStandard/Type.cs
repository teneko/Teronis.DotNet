using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;
using Teronis.Libraries.NetStandard;

namespace Teronis.Tools.NetStandard
{
    public static class TypeTools
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static object InstantiateUninitializedObject(Type type)
            => typeof(Instantiator<>).MakeGenericType(type).GetMethod(nameof(Instantiator<object>.Instantiate), BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

        public static object GetDefault(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type.IsValueType)
                return InstantiateUninitializedObject(type);
            else
                return null;
        }
    }
}
