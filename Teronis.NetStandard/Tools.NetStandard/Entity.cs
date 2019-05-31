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
            IncludeIfReadable = true,
            IncludeIfWritable = true,
        };

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, IEnumerable<MemberInfo> variableInfos)
        {
            foreach (var variable in variableInfos) {
                object value = null;

                try {
                    value = variable.GetValue(rightEntity);
                } catch (Exception error) {
                    throw new Exception($"A value cannot be retrieved from variable '{variable.Name}'", error);
                }

                try {
                    variable.SetValue(leftEntity, value);
                } catch (Exception error) {
                    throw new Exception($"The value '{value}' cannot be assigned to variable '{variable.Name}'", error);
                }
            }
        }

        // // BASE-TYPE-LOOP

        // NON-ATTRIBUTE

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, Type interruptingBaseType, VariableInfoSettings variableInfoSettings)
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? GetDefaultVariableInfoSettings();
            var variables = entityType.GetVariableMembers(interruptingBaseType, variableInfoSettings).ToList();
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
