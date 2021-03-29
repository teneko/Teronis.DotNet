// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Internals.Utils
{
    /// <summary>
    /// Internals.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Ensures that <paramref name="interfaceType"/> is assignable to 
        /// <typeparamref name="TAssignableTo"/>, otherwise an excpetion is
        /// thrown.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void EnsureInterfaceTypeIsAssignaleTo<TAssignableTo>(Type interfaceType)
        {
            if (!typeof(TAssignableTo).IsAssignableFrom(interfaceType)) {
                throw new ArgumentException($"The interface type must be assignable to {typeof(TAssignableTo)}.");
            }
        }

        /// <summary>
        /// Gets all the interfaces implemented or inherited.
        /// </summary>
        public static HashSet<Type> GetInterfaces(params Type[] types)
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
