using System;
using System.Reflection;
using System.Threading;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    internal class CancellableAnnotatedParameter : ParameterBase<CancellableAttribute>
    {
        public bool IsCancellationToken { get; private set; }
        public bool IsTimeSpan { get; private set; }

        public CancellableAnnotatedParameter(ParameterInfo parameterInfo, CancellableAttribute attribute)
            : base(parameterInfo, attribute) { }

        /// <summary>
        /// Reads parameter info.
        /// </summary>
        /// <exception cref="NotSupportedException">Invalid parameter type.</exception>
        protected internal override void ReadParameterInfo()
        {
            if (ParameterInfo.ParameterType == typeof(CancellationToken)) {
                IsCancellationToken = true;
            } else if (ParameterInfo.ParameterType == typeof(TimeSpan)) {
                IsTimeSpan = true;
            } else {
                throw new NotSupportedException($"The parameter type can only of type {typeof(CancellationToken)} and {typeof(TimeSpan)}.");
            }
        }
    }
}
