using System;
using System.Linq;
using System.Threading.Tasks;
using Teronis.Reflection;
using Teronis.Extensions.NetStandard;

namespace Teronis.Tools.NetStandard
{
    public static class GenericTools
    {
        public static bool ReturnNonDefault<T>(T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => !Tools.CompareEquality(outValue = inValue, default) || (FuncGenericTools.ReturnIsInvocable(getNonDefaultIfDefault, out outValue) && !Tools.CompareEquality(outValue, default));

        public static I ReturnInValue<I>(I inValue, out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Action<I> modifyInValue)
        {
            modifyInValue(inValue);
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Func<I, I> modifyInValue)
            => modifyInValue(inValue);

        public static I ReturnInValue<I>(I inValue, Action doSomething)
        {
            doSomething();
            return inValue;
        }

        public static async Task<I> ReturnInValue<I>(I inValue, Task task)
        {
            await task;
            return inValue;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, V value)
        {
            outInValue = inValue;
            return value;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<V> getValue)
        {
            outInValue = inValue;
            return getValue();
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<I, V> getValue)
        {
            outInValue = inValue;
            return getValue(inValue);
        }

        public static V ReturnValue<I, V>(I inValue, Func<I, V> getValue)
            => getValue(inValue);
        
        public static TCloningObjectType ShallowCopy<TCloningObjectType, TCopyingObject>(TCloningObjectType cloningObject, TCopyingObject copyingObject)
        {
            var cloningObjectVariableInfoSettings = new VariableInfoSettings() {
                IncludeIfWritable = true,
            };

            var clonedType = typeof(TCloningObjectType);

            var cloningObjectVariableInfoByNameList = clonedType
                .GetVariableInfos(cloningObjectVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var copyingObjectVariableInfoSettings = new VariableInfoSettings() {
                IncludeIfReadable = true,
            };

            var copyingObjectVariableInfoByNameList = typeof(TCopyingObject)
                .GetVariableInfos(copyingObjectVariableInfoSettings)
                .ToDictionary(x => x.Name);

            var serializerSettings = (TCloningObjectType)clonedType.InstantiateUninitializedObject();

            foreach (var nameAndCloningObjectVariableInfoPair in cloningObjectVariableInfoByNameList) {
                var cloningObjectVariableInfoKey = nameAndCloningObjectVariableInfoPair.Key;

                if (copyingObjectVariableInfoByNameList.ContainsKey(cloningObjectVariableInfoKey)) {
                    var cloningObjectVariableInfo = nameAndCloningObjectVariableInfoPair.Value;
                    var copyingObjectVariableInfo = copyingObjectVariableInfoByNameList[cloningObjectVariableInfoKey];
                    var copyingObjectVariableValue = copyingObjectVariableInfo.GetValue(copyingObject);

                    cloningObjectVariableInfo.SetValue(serializerSettings, copyingObjectVariableValue);
                }
            }

            return serializerSettings;
        }

        public static TCloningObjectType ShallowCopy<TCloningObjectType>(TCloningObjectType cloningObject)
            => ShallowCopy(cloningObject, cloningObject);
    }
}