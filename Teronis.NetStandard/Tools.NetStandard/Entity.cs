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

        // // BASE-TYPE-LOOP

        // NON-ATTRIBUTE

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, Type interruptingBaseType, VariableInfoSettings variableInfoSettings)
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? GetDefaultVariableInfoSettings();
            var variables = entityType.GetVariableMembers(interruptingBaseType, variableInfoSettings);
            UpdateEntityVariables(leftEntity, rightEntity, variables);
        }

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, Type interruptingBaseType)
            => UpdateEntityVariables(leftEntity, rightEntity, interruptingBaseType, default);

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, VariableInfoSettings settings)
            => UpdateEntityVariables(leftEntity, rightEntity, default, settings);

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity)
            => UpdateEntityVariables(leftEntity, rightEntity, default, default);

        // ATTRIBUTE

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType, Type interruptingBaseType, VariableInfoSettings variableInfoSettings)
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? GetDefaultVariableInfoSettings();
            var variables = entityType.GetAttributeVariableMembers(attributeType, variableInfoSettings).Select(x => x.MemberInfo);
            UpdateEntityVariables(leftEntity, rightEntity, variables);
        }

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType, Type interruptingBaseType)
            => UpdateEntityVariablesByAttribute(leftEntity, rightEntity, attributeType, interruptingBaseType, default);

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType, VariableInfoSettings settings)
            => UpdateEntityVariablesByAttribute(leftEntity, rightEntity, attributeType, default, settings);

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType)
            => UpdateEntityVariablesByAttribute(leftEntity, rightEntity, attributeType, default, default);
    }
}
