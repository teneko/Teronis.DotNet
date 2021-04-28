// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.Mappers.Description
{
    public class ProblemDetailsMapperDescriptorOptions
    {
        public ProblemDetailsMapperDescriptorOptions() { }

        public bool AllowDerivedMappableObjectTypes { get; set; }
        public IEnumerable<int>? StatusCodes { get; set; }
    }
}
