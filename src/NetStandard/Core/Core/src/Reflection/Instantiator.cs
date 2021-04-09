// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Reflection
{
    public static class Instantiator
    {
        /// <summary>
        /// Instantiates an object of type <paramref name="type"/>. If it has no default 
        /// constructor the object will be created uninitialized/without constructor.
        /// </summary>
        /// <param name="type">The type you want to instantiate.</param>
        /// <returns>The object that has been instantiated.</returns>
        /// <exception cref="NotSupportedException">
        /// The instantiation of passed type is not supported.
        /// </exception>
        public static object Instantiate(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            var instantiatorType = typeof(Instantiator<>);
            var genericInstantiatorType = instantiatorType.MakeGenericType(type);
            var instantiateMethodName = nameof(Instantiator<object>.Instantiate);
            var instantiateMethodBindingFlags = BindingFlags.Public | BindingFlags.Static;
            var instantiateMethod = genericInstantiatorType.GetMethod(instantiateMethodName, instantiateMethodBindingFlags)!;

            return instantiateMethod.Invoke(null, null)
                ?? throw new NotSupportedException($"The instantation of type {type} is not supported.");
        }

        /// <summary>
        /// Instantiates an object of type passed as argument. If it has no default 
        /// constructor the object will be created uninitialized/without constructor.
        /// </summary>
        /// <typeparam name="T">
        /// The type the instantiated object will be casted to.
        /// </typeparam>
        /// <returns>The object that has been instantiated.</returns>
        /// <exception cref="NotSupportedException">
        /// The instantiation of passed type is not supported.
        /// </exception>
        public static T Instantiate<T>(Type type) =>
            (T)Instantiate(type);

        /// <summary>
        /// Instantiates an object of type <typeparamref name="T"/>. If it has no default 
        /// constructor the object will be created uninitialized/without constructor.
        /// </summary>
        /// <typeparam name="T">The type you want to have instantiated.</typeparam>
        /// <returns>The object that has been instantiated.</returns>
        /// <exception cref="NotSupportedException">
        /// The instantiation of passed type is not supported.
        /// </exception>
        public static T Instantiate<T>()
            => (T)Instantiate(typeof(T));
    }
}
