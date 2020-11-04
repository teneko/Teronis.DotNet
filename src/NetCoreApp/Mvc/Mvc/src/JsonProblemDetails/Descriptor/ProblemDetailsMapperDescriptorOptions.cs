using System.Collections.Generic;

namespace Teronis.Mvc.JsonProblemDetails.Descriptor
{
    public class ProblemDetailsMapperDescriptorOptions
    {
        public ProblemDetailsMapperDescriptorOptions() { }

        public bool AllowDerivedMappableObjectTypes { get; set; }
        public IEnumerable<int>? StatusCodes { get; set; }
    }
}
