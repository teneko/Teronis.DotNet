// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    public class InterceptorValueAssignerContext : ValueAssignerContext
    {
        public InterceptorValueAssignerContext(ValueAssignerContext context)
            : base(context) { }

        public InterceptorValueAssignerContext(IValueAssigner propertyAssigner)
            : base(propertyAssigner) { }

        public InterceptorValueAssignerContext(IEnumerable<IValueAssigner> propertyAssigners)
            : base(propertyAssigners) { }

        public InterceptorValueAssignerContext(IEnumerable<IValueAssigner> propertyAssigners, int startIndex)
            : base(propertyAssigners, startIndex) { }

        public void SetInterceptorOriginatingValueResult(object? value)
        {
            ValueOriginatesFromInterceptor = true;
            ValueResult = value;
        }
    }
}
