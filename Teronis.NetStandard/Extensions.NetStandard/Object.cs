using System;
using System.Diagnostics;
using Teronis.Diagnostics;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class ObjectExtensions
    {
        public const string TO_DEBUG_STRING_METHOD_FULL_PATH = "{" + nameof(Teronis) + "." + nameof(Extensions) + "." + nameof(NetStandard) + "." + nameof(ObjectExtensions) + "." + nameof(ObjectExtensions.ToDebugString) + "(this)}";

        public static bool IsNullable(this object obj)
        {
            if (obj == null)
                return true; // obvious

            var type = obj.GetType();

            if (!type.IsValueType)
                return true; // ref-type

            if (Nullable.GetUnderlyingType(type) != null)
                return true; // Nullable<T>

            return false; // value-type
        }

        /// <summary>
        /// Casts any object to passed type.
        /// </summary>
        /// <typeparam name="T">Wished type</typeparam>
        /// <param name="obj">The object you want to be casted.</param>
        public static T CastTo<T>(this object obj) => (T)obj;

        public static object CastTo(this object obj, Type type)
        {
            var method = typeof(ObjectExtensions).GetMethod(nameof(CastTo), new[] { typeof(object) }).MakeGenericMethod(type);
            return method.Invoke(null, new object[] { obj });
        }

        /// <summary>
        /// Casts any object to passed type.
        /// </summary>
        /// <typeparam name="T">Wished type</typeparam>
        /// <param name="obj">The object you want to be casted.</param>
        public static T As<T>(this object obj) where T : class => obj as T;

        public static object As(this object obj, Type type)
        {
            var method = typeof(ObjectExtensions).GetMethod(nameof(As), new[] { typeof(object) }).MakeGenericMethod(type);
            return method.Invoke(null, new object[] { obj });
        }

        public static bool HasInterface<T>(this object obj) => obj != null && HasInterface<T>(obj.GetType());

        public static bool HasInterface<T>(this object obj, out T typedObj)
             => obj.HasInterface<T>() ? GenericTools.ReturnValue((T)obj, out typedObj, true) : GenericTools.ReturnValue(default, out typedObj, false);

        /// <summary>
        /// Looks for <see cref="IDebuggerDisplay"/> interface implementation. 
        /// If implemented, <see cref="IDebuggerDisplay.DebuggerDisplay"/> is returned,
        /// otherwise <see cref="object.ToString"/>.
        /// </summary>
        public static string ToDebugString(this object obj)
        {
            if (obj == null)
                new ArgumentNullException(nameof(obj));

            if (obj is IDebuggerDisplay debuggerDisplay)
                return debuggerDisplay.DebuggerDisplay;

            return obj.ToString();
        }
    }
}
