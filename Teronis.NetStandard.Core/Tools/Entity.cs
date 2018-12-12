using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Teronis.NetStandard.Extensions;

namespace Teronis.NetStandard.Tools
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

        //public static void UpdateModelEntity<T, A>(T leftEntity, T rightEntity, A filterByAttr)
        //    where A : Attribute
        //{
        //    var variableInfos = typeof(T).GetVar<A>(settings).Select(x => x.VarInfo);
        //    updateModelEntity(leftEntity, rightEntity, variableInfos);
        //}

        public static void UpdateModelEntity<T>(T leftEntity, T rightEntity, Type interruptAtBaseType = null)
        {
            var variableInfos = typeof(T).GetVariableInfos(getSettings(), interruptAtBaseType);
            UpdateModelEntity(leftEntity, rightEntity, variableInfos);
        }
    }
}
