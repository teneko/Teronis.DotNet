using System;
using System.Collections.Generic;
using System.Linq;

namespace Teronis.Microsoft.JSInterop.Dynamic.Utils
{
    internal static class TypeUtils
    {
        /// <summary>
        /// Gets all the interfaces implemented or inherited.
        /// </summary>
        internal static HashSet<Type> GetInterfaces(params Type[] types)
        {
            var interfaceTypeSet = new HashSet<Type>();

            if (types == null) {
                return interfaceTypeSet;
            }

            for (var index = 0; index < types.Length; index++) {
                var currentType = types[index];

                if (currentType == null) {
                    continue;
                }

                if (currentType.IsInterface) {
                    if (interfaceTypeSet.Add(currentType) == false) {
                        continue;
                    }
                }

                var currentTypeTheirInterfaceTypes = currentType.GetInterfaces();

                for (var i = 0; i < currentTypeTheirInterfaceTypes.Length; i++) {
                    var interfaceType = currentTypeTheirInterfaceTypes[i];
                    interfaceTypeSet.Add(interfaceType);
                }
            }

            return interfaceTypeSet;
        }

        public static void SortByName(Type[] array) =>
            Array.Sort(array, TypeOnlyNameComparer.Default);

        private sealed class TypeOnlyNameComparer : IComparer<Type>
        {
            public static readonly TypeOnlyNameComparer Default = new TypeOnlyNameComparer();

            public int Compare(Type? x, Type? y)
            {
                var assemblyQualifiedNameOfX = x?.AssemblyQualifiedName;
                var assemblyQualifiedNameOfY = y?.AssemblyQualifiedName;

                if (assemblyQualifiedNameOfX == assemblyQualifiedNameOfY) {
                    return 0;
                }

                if (assemblyQualifiedNameOfX is null) {
                    return -1;
                }

                return assemblyQualifiedNameOfX.CompareTo(assemblyQualifiedNameOfY);
            }
        }
    }
}
