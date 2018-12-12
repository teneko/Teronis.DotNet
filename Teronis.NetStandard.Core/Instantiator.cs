using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Teronis.NetStandard.Extensions;

namespace Teronis.NetStandard
{
    public class Instantiator<T>
    {
        public delegate Func<T> InstantiateDelegate();

        private static InstantiateDelegate instantiateDelegateReference;

        public static T Instantiate() => (instantiateDelegateReference ?? (instantiateDelegateReference = instantiate))()();

        private static Func<T> instantiate()
        {
            var type = typeof(T);

            if (type == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (type.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(type)).Compile(); // ~50 ms for classes and ~100 ms for structs

            return () => (T)FormatterServices.GetUninitializedObject(type); // ~2000 ms
        }
    }
}
