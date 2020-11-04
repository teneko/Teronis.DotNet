using System;
using System.Reflection;

namespace Teronis.Mvc.JsonProblemDetails.Reflection
{
    public class ParameterEvaluation
    {
        public ParameterInfo SourceInfo { get; }
        public virtual bool IsMapperContextParameter { get; }

        public ParameterEvaluation(ParameterInfo sourceInfo) =>
            SourceInfo = sourceInfo ?? throw new ArgumentNullException(nameof(sourceInfo));
    }
}
