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
        public static VariableInfoSettings GetDefaultVariableInfoSettings() => new VariableInfoSettings() {
            Flags = BindingFlags.Public | BindingFlags.Instance,
            IncludeIfReadable = true,
            IncludeIfWritable = true,
        };

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, IEnumerable<MemberInfo> variableInfos)
        {
            foreach (var varInfo in variableInfos)
                varInfo.SetValue(leftEntity, varInfo.GetValue(rightEntity));
        }

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, VariableInfoSettings variableInfoSettings = null, Type interruptAtBaseType = null)
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? GetDefaultVariableInfoSettings();
            var variables = entityType.GetVariableMembers(interruptAtBaseType, variableInfoSettings);
            UpdateEntityVariables(leftEntity, rightEntity, variables);
        }

        public static void UpdateEntityVariablesByAttribute<T, A>(T leftEntity, T rightEntity, VariableInfoSettings variableInfoSettings = null, Type interruptAtBaseType = null)
            where A : Attribute
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? GetDefaultVariableInfoSettings();
            var variables = entityType.GetAttributeVariableMembers<A>(variableInfoSettings).Select(x => x.MemberInfo);
            UpdateEntityVariables(leftEntity, rightEntity, variables);
        }

        /// <summary>
        /// Call this, when you cannot define T as Type, but need to define A as Type.
        /// </summary>
        public static void UpdateEntityVariablesByAttribute<T, A>(T leftEntity, T rightEntity, A attribute, VariableInfoSettings variableInfoSettings = null, Type interruptAtBaseType = null)
            where A : Attribute
            => UpdateEntityVariablesByAttribute<T, A>(leftEntity, rightEntity, variableInfoSettings, interruptAtBaseType);
    }
}
