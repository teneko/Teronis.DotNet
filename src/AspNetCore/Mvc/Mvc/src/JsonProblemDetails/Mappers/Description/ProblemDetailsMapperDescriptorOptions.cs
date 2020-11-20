using System.Collections.Generic;

namespace Teronis.Mvc.JsonProblemDetails.Mappers.Description
{
    public class ProblemDetailsMapperDescriptorOptions
    {
        public ProblemDetailsMapperDescriptorOptions() { }

        public bool AllowDerivedMappableObjectTypes { get; set; }
        public IEnumerable<int>? StatusCodes { get; set; }
    }
}
