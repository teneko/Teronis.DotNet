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
        /// <summary>
        /// <see cref="VariableInfoSettings.IncludeIfReadable"/> = true,
        /// <see cref="VariableInfoSettings.IncludeIfWritable"/> = true,
        /// <see cref="VariableInfoSettings.ExcludeByAttributeTypes"/> contains
        /// </summary>
        /// <returns></returns>
        public static VariableInfoSettings GetDefaultVariableInfoSettings() => new VariableInfoSettings() {
            IncludeIfReadable = true,
            IncludeIfWritable = true,
            ExcludeByAttributeTypes = new[] { typeof(IgnoreEntityVariableAttribute) }
        };

        #region Update Entity

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

        #endregion

        #region Shallow Copy

        public static TCloningObject ShallowCopy<TCloningObject, TCopyingObject>(TCopyingObject copyingObject)
        {
            var flags = VariableInfoSettings.DefaultFlags | BindingFlags.NonPublic;

            var cloningObjectMembersSettings = new VariableInfoSettings() {
                Flags = flags,
                IncludeIfWritable = true,
            };

            var declaredType = typeof(TCloningObject);

            var cloningObjectMembersByNameList = declaredType
                .GetVariableMembers(cloningObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var copyingObjectMembersSettings = new VariableInfoSettings() {
                Flags = flags,
                IncludeIfReadable = true,
            };

            var copyingObjectMembersByNameList = typeof(TCopyingObject)
                .GetVariableMembers(copyingObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var clonedObejct = (TCloningObject)declaredType.InstantiateUninitializedObject();

            foreach (var nameAndCloningObjectMembersPair in cloningObjectMembersByNameList) {
                var cloningObjectMembersKey = nameAndCloningObjectMembersPair.Key;

                if (copyingObjectMembersByNameList.ContainsKey(cloningObjectMembersKey)) {
                    var cloningObjectMember = nameAndCloningObjectMembersPair.Value;
                    var copyingObjectMember = copyingObjectMembersByNameList[cloningObjectMembersKey];

                    if (!cloningObjectMember.GetVariableType().IsAssignableFrom(copyingObjectMember.GetVariableType()))
                        continue;

                    var copyingObjectVariableValue = copyingObjectMember.GetValue(copyingObject);
                    cloningObjectMember.SetValue(clonedObejct, copyingObjectVariableValue);
                }
            }

            return clonedObejct;
        }

        public static TCloningAndCopyingObject ShallowCopy<TCloningAndCopyingObject>(TCloningAndCopyingObject copyingObject)
            => ShallowCopy<TCloningAndCopyingObject, TCloningAndCopyingObject>(copyingObject);

        #endregion

        #region Members

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);

            if (settings.Flags.HasFlag(BindingFlags.DeclaredOnly))
                interruptingBaseType = beginningType.BaseType;
            else {
                settings = settings.ShallowCopy();
                settings.Flags |= BindingFlags.DeclaredOnly;
                settings.Seal();
                interruptingBaseType = interruptingBaseType ?? typeof(object);
            }

            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var varInfo in getMembers(type, settings))
                    yield return varInfo;
        }

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
            => GetMembers(getMembers, beginningType, interruptingBaseType, default);

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
            => GetMembers(getMembers, beginningType, default, settings);

        #endregion

        #region Attribute Members 

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, settings))
                    if (propertyInfo.TryGetAttributeVariableMember(out AttributeMemberInfo<TAttribute> varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributeMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute
             => GetAttributeMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType)
             where TAttribute : Attribute
             => GetAttributeMembers<TAttribute>(getMembers, beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
             where TAttribute : Attribute
             => GetAttributeMembers<TAttribute>(getMembers, beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute
             => GetAttributeMembers<TAttribute>(getMembers, beginningType, default, default, getCustomAttributesInherit);

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, settings))
                    if (propertyInfo.TryGetAttributeVariableMember(attributeType, out var varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            => GetAttributeMembers(attributeType, getMembers, beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            => GetAttributeMembers(attributeType, getMembers, beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
             => GetAttributeMembers(attributeType, getMembers, beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType)
             => GetAttributeMembers(attributeType, getMembers, beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
             => GetAttributeMembers(attributeType, getMembers, beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, bool? getCustomAttributesInherit)
             => GetAttributeMembers(attributeType, getMembers, beginningType, default, default, getCustomAttributesInherit);

        #endregion
    }
}
