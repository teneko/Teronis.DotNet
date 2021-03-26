// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Teronis.Mvc.JsonProblemDetails.Reflection;

namespace Teronis.Mvc.JsonProblemDetails.Mappers.Description
{
    public class ProblemDetailsMapperDescriptor
    {
        public Type MapperType { get; }
        public bool AllowDerivedMapperSourceObjectTypes { get; }
        public IReadOnlyDictionary<MapperConstructorArea, ReadOnlyCollection<MapperConstructorEvaluation>> MapperConstructorsByArea { get; }
        public Type? MappableObjectType { get; }
        /// <summary>
        /// One or the other status code has to be contained 
        /// so that the descriptor is chosen. If null this
        /// property is ignored.
        /// </summary>
        public IReadOnlyCollection<int>? StatusCodes { get; }
        /// <summary>
        /// If true <see cref="StatusCodes"/> contains
        /// two sorted values which represents a range.
        /// </summary>
        public bool StatusCodesRepresentsRange { get; }

        private readonly SortedSet<int>? statusCodes;

        public ProblemDetailsMapperDescriptor(Type mapperType, bool allowDerivedMapperSourceObjectTypes, IEnumerable<int>? statusCodes)
        {
            MapperType = mapperType ?? throw new ArgumentNullException(nameof(mapperType));

            if (!typeof(IProblemDetailsMapper).IsAssignableFrom(mapperType)) {
                throw new ArgumentException($"The mapper type is not assignable to type {typeof(IProblemDetailsMapper).FullName}");
            }

            AllowDerivedMapperSourceObjectTypes = allowDerivedMapperSourceObjectTypes;
            MapperConstructorsByArea = ProblemDetailsMapperDescriptorUtils.FindAreaForEachConstructor(mapperType);
            var statusCodeSet = statusCodes is null ? null : new SortedSet<int>(statusCodes);
            this.statusCodes = statusCodeSet;
            StatusCodes = statusCodeSet;
            StatusCodesRepresentsRange = statusCodes is StatusCodeRange;
        }

        public bool IsStatusCodeSuitable(int? statusCode)
        {
            if (statusCode is null || statusCodes is null) {
                return true;
            }

            if (StatusCodesRepresentsRange) {
                var statusCodesEnumerator = statusCodes.GetEnumerator();
                statusCodesEnumerator.MoveNext();
                var begin = statusCodesEnumerator.Current;
                statusCodesEnumerator.MoveNext();
                var end = statusCodesEnumerator.Current;
                return begin <= statusCode && statusCode <= end;
            } else {
                return statusCodes.Contains(statusCode.Value);
            }
        }

        public bool TryFindConstructor(int startIndex, MapperConstructorArea area, Type? mappableObjectType,
            [NotNullWhen(true)] out (MapperConstructorEvaluation ConstructorEvaluation, int NextIndex)? foundConstructor,
            bool? globalAllowDerivedMappableObjectTypes = null)
        {
            if (!MapperConstructorsByArea.ContainsKey(area)) {
                goto exit;
            }

            var constructors = MapperConstructorsByArea[area];
            var constructorsCount = constructors.Count;

            for (var index = startIndex; index < constructorsCount; index++) {
                var constructorEvaluation = constructors[index];

                if (constructorEvaluation.MapperConstructorArea != area
                    || constructorEvaluation.FirstParameterEvaluation is null) {
                    goto exit;
                }

                if (constructorEvaluation.FirstParameterEvaluation.TryGetMapperContextParameterEvaluation(out var parameterEvaluation)
                    && parameterEvaluation.IsMappableObjectTypeSuitable(mappableObjectType, globalAllowDerivedMappableObjectTypes: globalAllowDerivedMappableObjectTypes)) {
                    foundConstructor = (constructorEvaluation, index + 1);
                    return true;
                }
            }

            exit:
            foundConstructor = null;
            return false;
        }
    }
}
