// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

namespace Teronis.Mvc.JsonProblemDetails.Reflection
{
    public class MapperContextParameterEvaluation : ParameterEvaluation
    {
        public override bool IsMapperContextParameter => true;
        public Type MappableObjectType { get; }
        public bool IsGenericMapperContextType { get; }
        public bool AllowDerivedMappableObjectTypes { get; }

        public MapperContextParameterEvaluation(ParameterInfo sourceInfo, Type mappableObjectType, bool isGenericMapperContextType)
            : base(sourceInfo)
        {
            MappableObjectType = mappableObjectType ?? throw new ArgumentNullException(nameof(mappableObjectType));
            IsGenericMapperContextType = isGenericMapperContextType;
            AllowDerivedMappableObjectTypes = sourceInfo.CustomAttributes.Any(x => x.AttributeType == typeof(AllowInheritancesAttribute));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappableObjectType"></param>
        /// <param name="globalAllowDerivedMappableObjectTypes">If <see cref="AllowDerivedMappableObjectTypes"/> 
        /// is false <paramref name="globalAllowDerivedMappableObjectTypes"/> is taken instead.</param>
        /// <returns></returns>
        public bool IsMappableObjectTypeSuitable(Type? mappableObjectType, bool? globalAllowDerivedMappableObjectTypes = null)
        {
            if (mappableObjectType is null) {
                return !IsGenericMapperContextType;
            }

            if (mappableObjectType == MappableObjectType) {
                return true;
            }

            if (!AllowDerivedMappableObjectTypes) {
                return false;
            }

            return MappableObjectType.IsAssignableFrom(mappableObjectType);
        }

    }
}
