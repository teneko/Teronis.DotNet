// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
