// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers.Description
{
    public static class ProblemDetailsMapperDescriptorOptionsExtensions
    {
        public static ProblemDetailsMapperDescriptorOptions AllowDerivedMappableObjectTypes(
            this ProblemDetailsMapperDescriptorOptions options,
            bool allowDerivedMapperSourceObjectTypes = true)
        {
            options.AllowDerivedMappableObjectTypes = allowDerivedMapperSourceObjectTypes;
            return options;
        }

        public static ProblemDetailsMapperDescriptorOptions WithStatusCodeRange(this ProblemDetailsMapperDescriptorOptions options,
            int beginStatusCode, int endStatusCode)
        {
            var statusCodeRange = new StatusCodeRange(beginStatusCode, endStatusCode);
            options.StatusCodes = statusCodeRange;
            return options;
        }

        public static ProblemDetailsMapperDescriptorOptions WithStatusCodeRange(this ProblemDetailsMapperDescriptorOptions options,
            HttpStatusCode beginStatusCode, HttpStatusCode endStatusCode) =>
            options.WithStatusCodeRange((int)beginStatusCode, (int)endStatusCode);

        public static ProblemDetailsMapperDescriptorOptions WithStatusCodes(this ProblemDetailsMapperDescriptorOptions options,
            IEnumerable<int>? statusCodes)
        {
            options.StatusCodes = statusCodes;
            return options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="statusCodes">The value null is assumed if empty.</param>
        /// <returns></returns>
        public static ProblemDetailsMapperDescriptorOptions WithStatusCodes(this ProblemDetailsMapperDescriptorOptions options,
            params int[]? statusCodes) =>
            options.WithStatusCodes(statusCodes is null || statusCodes.Length == 0 ? null : (IEnumerable<int>)statusCodes);
    }
}
