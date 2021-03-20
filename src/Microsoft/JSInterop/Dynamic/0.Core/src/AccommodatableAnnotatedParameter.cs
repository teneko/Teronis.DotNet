using System;
using System.Collections;
using System.Reflection;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal class AccommodatableAnnotatedParameter : ParameterBase<AccommodatableAttribute>
    {
        public AccommodatableAnnotatedParameter(ParameterInfo parameterInfo, AccommodatableAttribute attribute)
            : base(parameterInfo, attribute) { }

        /// <summary>
        /// Reads parameter info.
        /// </summary>
        /// <exception cref="NotSupportedException">Invalid parameter type.</exception>
        protected internal override void ReadParameterInfo()
        {
            if (!typeof(IEnumerable).IsAssignableFrom(ParameterInfo.ParameterType)) {
                throw new NotSupportedException($"The parameter type has to implement {typeof(IEnumerable)}.");
            }
        }
    }
}
