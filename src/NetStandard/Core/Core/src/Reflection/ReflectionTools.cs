using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teronis.Extensions;
using Teronis.Reflection;

namespace Teronis.Tools
{
    public static class ReflectionTools
    {
        // Property <see cref="VariableInfoSettings.ExcludeByAttributeTypes"/> contains <see cref="IgnoreEntityVariableAttribute"/>.
        /// <summary>
        /// Property <see cref="VariableInfoSettings.IncludeIfReadable"/> is true.
        /// Property <see cref="VariableInfoSettings.IncludeIfWritable"/> is true.
        /// </summary>
        private static VariableInfoDescriptor createDefaultVariableInfoSettings()
        {
            return new VariableInfoDescriptor() {
                IncludeIfReadable = true,
                IncludeIfWritable = true,
            };
        }

        #region Update Entity Variables

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

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, VariableInfoDescriptor leftVariablesDescriptor = null, VariableInfoDescriptor rightVariablesDescriptor = null)
        {
            var leftEntityType = leftEntity?.GetType();
            var rightEntityType = rightEntity?.GetType();

            if (leftEntityType == null || rightEntityType == null)
                return;

            leftVariablesDescriptor = leftVariablesDescriptor ?? createDefaultVariableInfoSettings();
            var leftEntityVariableMembers = leftEntityType.GetVariableMembers(descriptor: leftVariablesDescriptor);
            var intersectedEntityVariableMembers = leftEntityVariableMembers.Intersect(rightEntityType, rightVariablesDescriptor);
            UpdateEntityVariables(leftEntity, rightEntity, intersectedEntityVariableMembers);
        }

        // ATTRIBUTE

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType, VariableInfoDescriptor variableInfoSettings = null)
        {
            var entityType = leftEntity?.GetType();

            if (entityType == null)
                return;

            variableInfoSettings = variableInfoSettings ?? createDefaultVariableInfoSettings();

            var leftEntityVariableMembers = entityType.GetAttributeVariableMembers(attributeType, descriptor: variableInfoSettings)
                .Select(x => x.MemberInfo);

            UpdateEntityVariables(leftEntity, rightEntity, leftEntityVariableMembers);
        }

        #endregion

        #region Shallow Copy

        public static TargetType ShallowCopy<TargetType, SourceType>(SourceType source)
        {
            var flags = VariableInfoDescriptor.DefaultFlags | BindingFlags.NonPublic;

            var cloningObjectMembersSettings = new VariableInfoDescriptor() {
                Flags = flags,
                IncludeIfWritable = true,
            };

            var declaredType = typeof(TargetType);

            var cloningObjectMembersByNameList = declaredType
                .GetVariableMembers(descriptor: cloningObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var copyingObjectMembersSettings = new VariableInfoDescriptor() {
                Flags = flags,
                IncludeIfReadable = true,
            };

            var copyingObjectMembersByNameList = typeof(SourceType)
                .GetVariableMembers(descriptor: copyingObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var clonedObejct = (TargetType)declaredType.InstantiateUninitializedObject();

            foreach (var nameAndCloningObjectMembersPair in cloningObjectMembersByNameList) {
                var cloningObjectMembersKey = nameAndCloningObjectMembersPair.Key;

                if (copyingObjectMembersByNameList.ContainsKey(cloningObjectMembersKey)) {
                    var cloningObjectMember = nameAndCloningObjectMembersPair.Value;
                    var copyingObjectMember = copyingObjectMembersByNameList[cloningObjectMembersKey];

                    if (!cloningObjectMember.GetVariableType().IsAssignableFrom(copyingObjectMember.GetVariableType()))
                        continue;

                    var copyingObjectVariableValue = copyingObjectMember.GetValue(source);
                    cloningObjectMember.SetValue(clonedObejct, copyingObjectVariableValue);
                }
            }

            return clonedObejct;
        }

        public static T ShallowCopy<T>(T entity)
            => ShallowCopy<T, T>(entity);

        #endregion

        #region Members

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor variableInfoDescriptor = null)
        {
            if (beginningType is null) {
                yield break;
            }

            variableInfoDescriptor = variableInfoDescriptor.DefaultIfNull(true);

            if (variableInfoDescriptor.Flags.HasFlag(BindingFlags.DeclaredOnly))
                interruptingBaseType = beginningType.BaseType;
            else {
                variableInfoDescriptor = variableInfoDescriptor.ShallowCopy();
                variableInfoDescriptor.Flags |= BindingFlags.DeclaredOnly;
                variableInfoDescriptor.Seal();
                interruptingBaseType = interruptingBaseType ?? typeof(object);
            }

            var basesTypes = beginningType.GetBaseTypes(interruptingBaseType);

            foreach (var type in basesTypes) {
                foreach (var varInfo in getMembers(type, variableInfoDescriptor)) {
                    yield return varInfo;
                }
            }
        }

        #endregion

        #region Attribute Members 

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor variableInfoDescriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, variableInfoDescriptor))
                    if (propertyInfo.TryGetAttributeVariableMember(out AttributeMemberInfo<TAttribute> varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor variableInfoDescriptor = null, bool? getCustomAttributesInherit = null)
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, variableInfoDescriptor))
                    if (propertyInfo.TryGetAttributeVariableMember(attributeType, out var varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        #endregion
    }
}
