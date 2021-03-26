// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Text.Json.Serialization
{
    public class VariablesClusionHelper : IVariablesClusionHelper
    {
        public readonly Dictionary<Type, HashSet<string>> VariablesByTypeList;

        public VariablesClusionHelper() =>
            VariablesByTypeList = new Dictionary<Type, HashSet<string>>();

        public VariablesClusionHelper(IEnumerable<KeyValuePair<Type, string>> includedVariables)
            : this()
        {
            var keyValueKeyPairCollection = (ICollection<KeyValuePair<Type, string>>)VariablesByTypeList;

            foreach (var keyValuePair in includedVariables) {
                keyValueKeyPairCollection.Add(keyValuePair);
            }
        }

        public void ConsiderVariable(Type declaringType, params string[] propertyName)
        {
            declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            // Start bucket if does not exist
            if (!VariablesByTypeList.ContainsKey(declaringType)) {
                VariablesByTypeList[declaringType] = new HashSet<string>();
            }

            foreach (var prop in propertyName) {
                VariablesByTypeList[declaringType].Add(prop);
            }
        }

        /// <summary>
        /// Is the given property for the given type included/excluded?
        /// </summary>
        public bool IsVariableConsidered(Type declaringType, string propertyName)
        {
            declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));

            bool isVariableConsidered(Type type, string propertyName)
            {
                if (VariablesByTypeList.ContainsKey(type) && (VariablesByTypeList[type].Count == 0 || VariablesByTypeList[type].Contains(propertyName))) {
                    return true;
                } else {
                    return false;
                }
            }

            /// Need to check base type as well for EF -- @per comment by user576838
            if (isVariableConsidered(declaringType, propertyName) || (declaringType.BaseType != null && isVariableConsidered(declaringType.BaseType, propertyName))) {
                return true;
            } else {
                return false;
            }
        }
    }
}
