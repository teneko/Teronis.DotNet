// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Teronis.Utils;

namespace Teronis.Reflection
{
    public class Instantiator<T>
    {
        private delegate Func<T> InstantiateDelegate();

        private static InstantiateDelegate? instantiateDelegateReference;

        private static Func<T> instantiate()
        {
            var type = typeof(T);

            if (type == typeof(string)) {
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();
            }
            
            if (TypeUtils.HasDefaultConstructor(type)) {
                return Expression.Lambda<Func<T>>(Expression.New(type)).Compile(); // ~50 ms for classes and ~100 ms for structs
            }

            return () => (T)FormatterServices.GetUninitializedObject(type); // ~2000 ms
        }

        /// <summary>
        /// Instantiates an object of type <typeparamref name="T"/>. If it has no default 
        /// constructor the object will be created uninitialized/without constructor.
        /// </summary>
        /// <returns>The object that has been instantiated.</returns>
        public static T Instantiate()
        {
            if (instantiateDelegateReference == null) {
                instantiateDelegateReference = instantiate;
            }

            return instantiateDelegateReference()();
        }
    }
}
