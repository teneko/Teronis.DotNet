using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;

namespace Teronis.Tools.NetStandard
{
    public static class EntityTools
    {
        private static VariableInfoSettings getSettings() => new VariableInfoSettings()
        {
            Flags = BindingFlags.Public | BindingFlags.Instance,
            RequireReadability = true,
            RequireWritablity = true,
        };

        public static void UpdateModelEntity<T>(T leftEntity, T rightEntity, IEnumerable<VariableInfo> variableInfos)
        {
            foreach (var varInfo in variableInfos)
                varInfo.SetValue(leftEntity, varInfo.GetValue(rightEntity));
        }

        public static void UpdateModelEntity<T, A>(T leftEntity, T rightEntity, Type interruptAtBaseType = null)
            where A : Attribute
        {
            var variableInfos = typeof(T).GetAttributeVariableInfos<A>(getSettings()).Select(x => x.VariableInfo);
            UpdateModelEntity(leftEntity, rightEntity, variableInfos);
        }

        /// <summary>
        /// Call this, when you cannot define T as Type, but need to define A as Type.
        /// </summary>
        public static void UpdateModelEntity<T, A>(T leftEntity, T rightEntity, A attribute, Type interruptAtBaseType = null)
            where A : Attribute
            => UpdateModelEntity<T, A>(leftEntity, rightEntity, interruptAtBaseType);

        public static void UpdateModelEntity<T>(T leftEntity, T rightEntity, Type interruptAtBaseType = null)
        {
            var variableInfos = typeof(T).GetVariableInfos(interruptAtBaseType, getSettings());
            UpdateModelEntity(leftEntity, rightEntity, variableInfos);
        }
    }
}
